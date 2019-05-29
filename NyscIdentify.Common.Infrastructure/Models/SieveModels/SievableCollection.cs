using CommonServiceLocator;
using NyscIdentify.Common.Infrastructure.Models.SieveModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Mvvm;
using RestSharp;
using RestSharp.Authenticators;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sieve.Models;
using System.Threading;
using System.IO;
using NyscIdentify.Common.Infrastructure.Resources.Collections;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using Fody = PropertyChanged;
using System.Windows.Threading;

namespace NyscIdentify.Common.Infrastructure.Models.SieveModels
{
    public class SievableCollection<T> : BindableBase,
    IWaitable where T : class
    {
        #region Properties

        #region Statics
        static string[] AutoUpdateProperties = new string[]
        {
            nameof(SearchQuery), nameof(DataSource),
            nameof(PageSize)
        };
        #endregion

        #region Events
        event ListChangedEventHandler ListChangedHandler;
        #endregion


        #region Bindables

        #region IWaitable Implementation
        bool _isBusy = false;
        [PropertyChanged.DoNotNotify]
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public string BusyMessage { get; private set; }
        public double Progress { get; private set; } = 50.0;
        #endregion




        public string DataSource { get; set; }

        int _pageNumber = 1;
        [Fody.DoNotNotify]
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                SetProperty(ref _pageNumber, value);

                PageNumberTimer.Stop();
                PageNumberTimer.Start();
            }
        }


        public int PageSize { get; set; } = 10;

        [Fody.AlsoNotifyFor(nameof(PageNumber))]
        public bool CanPrevious => PageNumber > 1;
        [Fody.AlsoNotifyFor(nameof(PageNumber))]
        public bool CanNext => PageNumber < PageCount;

        public int PageCount
        {
            get
            {
                int count = TotalCount;
                if (count == 0) return 1;
                return count % PageSize != 0
                    ? count / PageSize + 1
                    : count / PageSize;
            }
        }

        public int TotalCount { get; set; }
        public string SearchQuery { get; set; }

        #region Collections
        public AsyncObservableCollection<T> Data { get; } = new AsyncObservableCollection<T>();
        public BindingList<SieveSortTerm> Sorts { get; } = new BindingList<SieveSortTerm>();
        public BindingList<SieveFilterTerm> Filters { get; } = new BindingList<SieveFilterTerm>();


        public ObservableCollection<SieveProperty> SortableProperties { get; } = new ObservableCollection<SieveProperty>();
        public ObservableCollection<SieveProperty> FilterableProperties { get; } = new ObservableCollection<SieveProperty>();

        public ObservableCollection<SieveFilterOperator> FilterOperators { get; } = ((SieveFilterOperator)0).ToObservable();
        public ObservableCollection<SieveSortOperator> SortOperators { get; } = ((SieveSortOperator)0).ToObservable();

        public AsyncObservableCollection<int> Pages { get; set; } = new AsyncObservableCollection<int>();
        #endregion

        #endregion


        public JsonSerializerSettings SerializationSettings { get; set; } = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            TypeNameHandling = TypeNameHandling.All
        };

        #region Internals

        #region Flags
        [PropertyChanged.DoNotNotify]
        bool NewDataPaging { get; set; }

        [PropertyChanged.DoNotNotify]
        bool IsReallyBusy { get; set; }

        [PropertyChanged.DoNotNotify]
        bool UpdateNeeded { get; set; } = true;

        [PropertyChanged.DoNotNotify]
        bool Updating { get; set; }
        #endregion

        RestSharp.RestClient Client { get; } = new RestSharp.RestClient();
        JsonSerializer Serializer { get; } = new JsonSerializer();
        CancellationTokenSource CancellationElement { get; set; } = new CancellationTokenSource();

        DispatcherTimer PageNumberTimer { get; } = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.DataBind, (s, e) =>
        {
            if (!(s is SievableCollection<T> collection)) return;

            Task.Run(() => collection.UpdateData(true));
        }, Core.Dispatcher);
        #endregion

        #endregion

        #region Constructors
        public SievableCollection(string dataSource)
        {
            Initialize();
            DataSource = dataSource;

            RegisterEvents();
        }
        #endregion

        #region Methods
        void Initialize()
        {
            var properties = typeof(T).GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.GetProperty | BindingFlags.SetProperty |
                BindingFlags.Instance).
                Where(x => Attribute.IsDefined(x, typeof(SieveAttribute)));

            foreach (var prop in properties)
            {
                var sieve = (SieveAttribute)Attribute.
                    GetCustomAttribute(prop, typeof(SieveAttribute));

                if (string.IsNullOrWhiteSpace(sieve.Name)) sieve.Name = prop.Name;
                var property = new SieveProperty(sieve.Name, prop.Name);

                if (sieve.CanSort) SortableProperties.Add(property);
                if (sieve.CanFilter) FilterableProperties.Add(property);
            }
        }

        void RegisterEvents()
        {
            ListChangedHandler = (s, e) => UpdateNeeded = true;

            Sorts.ListChanged += ListChangedHandler;
            Filters.ListChanged += ListChangedHandler;

            Filters.ListChanged += (s, e) =>
            {
                if (!Updating) NewDataPaging = true;
            };
        }

        public async Task UpdateData(bool force = false)
        {
            if (IsReallyBusy) return;
            if (IsBusy) CancellationElement.Cancel();

            if (!UpdateNeeded && !force) return;

            Updating = true;
            await WaitTillIdle();
            CancellationElement.Dispose();
            CancellationElement = new CancellationTokenSource();

            Core.Dispatcher.Invoke(() => IsBusy = true);
            await Update(CancellationElement.Token);
            await Core.Dispatcher.InvokeAsync(() => 
            {
                IsBusy = false;
                RaisePropertyChanged(nameof(PageCount));
                RaisePropertyChanged(nameof(Pages));
                RaisePropertyChanged(nameof(PageNumber));
            });
            UpdateNeeded = Updating = false;
        }

        async Task Update(CancellationToken cancellationToken)
        {
            Client.Authenticator = ServiceLocator
                .Current.GetInstance<IAuthenticator>();
            RestSharp.IRestResponse response = null;
            
            try
            {
                RestSharp.IRestRequest request = new RestRequest(DataSource, RestSharp.Method.GET);

                string sorts = string.Join(",", Sorts.Select(s => s.ToString()));
                string filters = string.Join(",", Filters.Select(f => f.ToString()));

                string page = NewDataPaging ? "1" : PageNumber.ToString();
                NewDataPaging = false;

                request.AddQueryParameter("searchQuery", SearchQuery);
                request.AddQueryParameter("page", page);
                request.AddQueryParameter("pageSize", PageSize.ToString());
                request.AddQueryParameter("sorts", sorts);
                request.AddQueryParameter("filters", filters);

                Core.Log.Debug($"Page: {PageNumber}\nPage Size: {PageSize}" +
                    $"\nSorts: {sorts}\nFilters: {filters}");

                response = await Client.ExecuteTaskAsync(request, cancellationToken);

                if (cancellationToken.IsCancellationRequested) return;
                Core.Log.Debug(response.Content);

                var data = JsonConvert.DeserializeObject<List<T>>(response.Content, SerializationSettings);

                if (data == null) return;

                Data.Clear();
                await Data.AddRangeAsync(data);
                var total = response.Headers.FirstOrDefault(p => p.Name == "X-Total-Count").Value;


                TotalCount = Math.Max(Convert.ToInt32(total), 1);
                PageNumber = Math.Max(Convert.ToInt32(page), 1);

                Pages.Clear();
                await Pages.AddRangeAsync(Enumerable.Range(1, PageCount).ToArray());
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured during a sieve collection update.\n{ex}");
            }
        }

        async Task WaitTillIdle()
        {
            IsReallyBusy = true;
            while (IsBusy) await Task.Delay(200);
            IsReallyBusy = false;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            
            if (!Updating)
            {
                if (args.PropertyName == nameof(SearchQuery) || 
                    args.PropertyName == nameof(PageSize))
                    NewDataPaging = true;

                UpdateNeeded = true;
                if (AutoUpdateProperties.Contains(args.PropertyName))
                    Task.Run(() => UpdateData());
            }
        }

        public void AddFilterTerm() =>
            Filters.Add(new SieveFilterTerm()
            { Property = FilterableProperties.FirstOrDefault() });

        public void RemoveFilterItem(SieveFilterTerm item) =>
            Filters.Remove(item);

        public void AddSortTerm() =>
            Sorts.Add(new SieveSortTerm()
            { Property = SortableProperties.FirstOrDefault() });

        public void RemoveSortTerm(SieveSortTerm term) =>
            Sorts.Remove(term);
        
        #endregion
    }
}
