using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Resources.Collections
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        #region Properties
        SynchronizationContext Context { get; } = SynchronizationContext.Current;
        public bool SuppressNotification { get; set; } = false;
        #endregion

        #region Constructors
        public AsyncObservableCollection() { }
        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }
        #endregion

        #region Methods
        public virtual void AddRange(IEnumerable<T> list, bool notify = true)
        {
            if (list == null) return;
            SuppressNotification = true;

            foreach (T item in list) Add(item);
            SuppressNotification = !notify;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public async Task AddRangeAsync(IEnumerable<T> list, bool notify = true)
        {
            if (list == null) return;
            SuppressNotification = true;

            bool complete = false;

            while (!complete)
            {
                try
                {
                    foreach (T item in list) Add(item);
                    complete = true;
                }
                catch { await Task.Delay(100); }
            }

            SuppressNotification = !notify;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #region Overrides
        public void Reset() => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SuppressNotification) return;
            if (Context == SynchronizationContext.Current) RaiseCollectionChanged(e);
            else Context.Send(RaiseCollectionChanged, e);
        }

        private void RaiseCollectionChanged(object param) => base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (Context == SynchronizationContext.Current) RaisePropertyChanged(e);
            else Context.Send(RaisePropertyChanged, e);
        }

        private void RaisePropertyChanged(object param) => base.OnPropertyChanged((PropertyChangedEventArgs)param);
        #endregion

        #endregion
    }
}
