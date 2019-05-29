using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class SingleIdentityViewModel : BindableBase, INavigationAware, IWaitable
    {
        #region Properties

        #region Services
        [DeepDependency]
        IIdentityManager IdentityManager { get; }
        [DeepDependency]
        IViewManager ViewManager { get; }
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IAlertService AlertService { get; }
        #endregion

        #region Commands
        public ICommand BackCommand { get; }

        public ICommand OpenDialogCommand { get; }
        public ICommand CloseDialogCommand { get; }

        public ICommand ApproveCommand { get; }
        public ICommand RevokeCommand { get; }
        public ICommand DeclineCommand { get; }

        public ICommand ViewTimelineCommand { get; }
        #endregion

        #region Bindables
        [UpdateWith(Source = nameof(IdentityManager), Property = nameof(SelectedIdentity))]
        public User SelectedIdentity => IdentityManager.SelectedIdentity;
        public bool DialogActive { get; set; }
        public string DeclineMessage { get; set; } = string.Empty;
        public double DeclineMessageLimit { get; } = 200;

        [UpdateWith(Source = nameof(ViewManager), Property = nameof(IViewManager.BackstageActive))]
        public bool BackstageActive { get => ViewManager.BackstageActive; set => ViewManager.BackstageActive = value; }

        public ObservableCollection<SimpleUserActivity> UserActivities { get; } = new ObservableCollection<SimpleUserActivity>();

        #region IWaitable Implementation
        public bool IsBusy { get; set; } = false;
        public string BusyMessage { get; set; } = "Please Wait...";
        public double Progress => 50;
        #endregion

        #endregion

        #endregion

        #region Methods

        #region INavigationAware Implementation
        public void OnNavigatedTo(NavigationContext navigationContext) => RaisePropertyChanged(nameof(SelectedIdentity));

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion

        #region Command Handlers
        [CommandMethod(nameof(BackCommand))]
        void OnBack() => ViewManager.ActiveTab = TabMenu.Home;

        [CommandMethod(nameof(OpenDialogCommand))]
        void OnOpenDialog()
        {
            //DeclineMessage = string.Empty;
            DialogActive = true;
        }

        [CommandMethod(nameof(CloseDialogCommand))]
        void OnCloseDialog() => DialogActive = false;
        
        [CommandMethod(nameof(ApproveCommand))]
        void OnApproval()
        {
            DialogActive = false;
            if (SelectedIdentity == null) return;

            Task.Run(async () =>
            {
                IsBusy = true;
                BusyMessage = $"Approving {SelectedIdentity.FullName}...";

                bool approved = await IdentityManager.ApproveIdentity();
                BusyMessage = "Updating Details...";
                if (approved) await UpdateIdentity();
                IsBusy = false;
            });
        }

        [CommandMethod(nameof(RevokeCommand))]
        void OnRevocation()
        {
            DialogActive = false;
            if (SelectedIdentity == null) return;

            Task.Run(async () =>
            {
                IsBusy = true;
                BusyMessage = $"Revoking {SelectedIdentity.FullName}...";

                bool declined = await IdentityManager.RevokeApprovalStatus();
                BusyMessage = "Updating Details...";
                if (declined) await UpdateIdentity();
                IsBusy = false;
            });
        }

        [CommandMethod(nameof(DeclineCommand))]
        void OnRejection()
        {
            DialogActive = false;
            if (SelectedIdentity == null) return;

            Task.Run(async () =>
            {
                IsBusy = true;
                BusyMessage = $"Declining {SelectedIdentity.FullName}...";

                bool declined = await IdentityManager.DeclineIdentity(DeclineMessage);
                BusyMessage = "Updating Details...";
                if (declined) await UpdateIdentity();
                IsBusy = false;
            });
        }

        void OnViewTimeline()
        {
            
        }
        #endregion

        async Task UpdateIdentity()
        {
            User identity = await IdentityManager.GetIdentity();

            if (identity != null)
            {
                var initial = IdentityManager.Online.Data.
                    FirstOrDefault(u => u.Id == identity.Id);
                int index = IdentityManager.Online.Data.IndexOf(initial);

                if (index > 0)
                    IdentityManager.Online.Data[index] = identity;
                IdentityManager.SelectedIdentity = identity;
            }
        }

        #endregion
    }
}
