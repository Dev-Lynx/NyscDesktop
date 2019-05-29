using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.SieveModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using NyscIdentify.Desktop.Views;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity.Attributes;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class IdentityViewModel : BindableBase
    {
        #region Properties

        #region Bindables
        [UpdateWith(Source = nameof(IdentityManager), Property = nameof(Online))]
        public SievableCollection<User> Online => IdentityManager.Online;
        
        [UpdateWith(Source = nameof(IdentityManager), Property = nameof(SelectedIdentity))]
        public User SelectedIdentity
        {
            get => IdentityManager.SelectedIdentity;
            set => IdentityManager.SelectedIdentity = value;
        }

        public Layout Layout { get; private set; }
        #endregion

        #region Services
        [DeepDependency]
        IIdentityManager IdentityManager { get; }
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IRegionManager RegionManager { get; }
        [DeepDependency]
        IViewManager ViewManager { get; }
        #endregion

        #region Commands
        public ICommand AddFilterCommand { get; }
        public ICommand RemoveFilterCommand { get; }
        public ICommand AddSortCommand { get; }
        public ICommand RemoveSortCommand { get; }
        public ICommand UpdateTableCommand { get; }
        public ICommand ForceTableUpdateCommand { get; }
        public ICommand ChangeLayoutCommand { get; }
        public ICommand ViewIdentityCommand { get; }

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        #endregion

        #endregion

        #region Methods
        [CommandMethod(nameof(AddFilterCommand))]
        void OnAddFilter() => IdentityManager.Online.AddFilterTerm();

        [CommandMethod(nameof(RemoveFilterCommand), true)]
        void OnRemoveFilter(object obj)
        {
            int[] c = new int[5];
            if (!(obj is SieveFilterTerm)) return;
            IdentityManager.Online.RemoveFilterItem((SieveFilterTerm)obj);
        }

        [CommandMethod(nameof(AddSortCommand))]
        void OnAddSort() => IdentityManager.Online.AddSortTerm();

        [CommandMethod(nameof(RemoveSortCommand), true)]
        void OnRemoveSort(object obj)
        {
            if (!(obj is SieveSortTerm)) return;
            IdentityManager.Online.RemoveSortTerm((SieveSortTerm)obj);
        }

        [CommandMethod(nameof(UpdateTableCommand))]
        void OnUpdateTable() => Task.Run(() => IdentityManager.Online.UpdateData());

        [CommandMethod(nameof(ForceTableUpdateCommand))]
        void OnForceUpdateTable() => Task.Run(() => IdentityManager.Online.UpdateData(true));

        [CommandMethod(nameof(ChangeLayoutCommand))]
        void OnChangeLayout() => Layout = Layout.Next();

        [CommandMethod(nameof(ViewIdentityCommand))]
        void OnViewIdentity()
        {
            if (SelectedIdentity == null) return;

            ViewManager.ActiveTab = TabMenu.SingleView;
            // RegionManager.NavigateToView<SingleIdentityView>(Core.MAIN_REGION);
        }

        [CommandMethod(nameof(PreviousPageCommand))]
        void OnPreviousPage()
        {
            Online.PageNumber--;
        }

        [CommandMethod(nameof(NextPageCommand))]
        void OnNextPage()
        {
            Online.PageNumber++;
        }
        #endregion
    }
}
