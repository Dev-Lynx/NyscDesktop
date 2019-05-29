using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Models;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
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
using Fody = PropertyChanged;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class ShellViewModel : BindableBase
    {
        #region Properties

        #region Bindables
        [UpdateWith(Source = nameof(AccountManager), Property = nameof(IAccountManager.CurrentUser))]
        public User CurrentUser => AccountManager.CurrentUser;

        [UpdateWith(Source = nameof(AlertService), Property = nameof(IAlertService.Context))]
        public AlertContext AlertContext => AlertService.Context;

        [UpdateWith(Source = nameof(ViewManager), Property = nameof(IViewManager.ActiveTab))]
        public TabMenu ActiveTab { get => ViewManager.ActiveTab; set => ViewManager.ActiveTab = value; }

        [UpdateWith(Source = nameof(IdentityManager), Property = nameof(IIdentityManager.SelectedIdentity))]
        public User SelectedIdentity
        {
            get => IdentityManager.SelectedIdentity;
            set => IdentityManager.SelectedIdentity = value;
        }

        [UpdateWith(Source = nameof(IdentityManager), 
            Property = nameof(IIdentityManager.SelectedIdentity))]
        public bool CanViewIdentity => SelectedIdentity != null;

        [UpdateWith(Source = nameof(ViewManager), Property = nameof(IViewManager.BackstageActive))]
        public bool BackstageActive { get => ViewManager.BackstageActive; set => ViewManager.BackstageActive = value; }

        public bool AccountDetailsActive { get; private set; }
        #endregion

        #region Services
        [DeepDependency]
        IIdentityManager IdentityManager { get; }
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IConfigurationManager ConfigurationManager { get; }
        [DeepDependency]
        IAccountManager AccountManager { get; }
        [DeepDependency]
        IAlertService AlertService { get; }
        [DeepDependency]
        IViewManager ViewManager { get; }
        [DeepDependency]
        IRegionManager RegionManager { get; }
        #endregion

        #region Commands
        public ICommand SignOutCommand { get; }
        public ICommand ToggleAccountDetails { get; }
        public ICommand AccountSettingsCommand { get; }

        public ICommand ShutdownCommand { get; }

        #region Menu Commands
        public ICommand ViewIdentityCommand { get; }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region Command Handlers
        [CommandMethod(nameof(SignOutCommand))]
        void OnLogOut() => AccountManager.LogOut();
        [CommandMethod(nameof(ShutdownCommand))]
        void OnExit() => Core.App.Shutdown();

        [CommandMethod(nameof(ToggleAccountDetails))]
        void OnToggleAccountDetails() => AccountDetailsActive = !AccountDetailsActive;

        [CommandMethod(nameof(AccountSettingsCommand))]
        void OnNavigateToAccountSettings()
        {
            AccountDetailsActive = false;
            ViewManager.BackstageActive = true;
        }

        #region Menu Command Handlers
        [CommandMethod(nameof(ViewIdentityCommand))]
        void OnViewIdentity() => ViewManager.ActiveTab = TabMenu.SingleView;
        #endregion

        #endregion

        #endregion
    }
}
