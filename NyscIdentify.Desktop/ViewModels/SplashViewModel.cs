using CommonServiceLocator;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Unity.Attributes;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Data;
using Fody = PropertyChanged;
using System.Windows;
using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class SplashViewModel : BindableBase
    {
        #region Properties

        #region Bindables
        public ObservableCollection<UserRole> UserRoles { get; } = ((UserRole)0).ToObservable();
        public UserLoginViewModel LoginModel { get; private set; } = new UserLoginViewModel();

        SecureString _password = new SecureString();
        [Fody.DoNotNotify]
        public SecureString Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                if (!string.IsNullOrWhiteSpace(LoginError)) LoginError = null;
            }
        }

        public string LoginError { get; set; }

        [UpdateWith(Source = nameof(AccountManager), Property = nameof(IsBusy))]
        public bool IsBusy => AccountManager.IsBusy;
        [UpdateWith(Source = nameof(AccountManager), Property = nameof(BusyMessage))]
        public string BusyMessage => AccountManager.BusyMessage;
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand CloseCommand { get; }
        #endregion

        #region Services
        [DeepDependency]
        IAccountManager AccountManager { get; }
        [DeepDependency]
        IConnectionService ConnectionService { get; }
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IUnityContainer Container { get; }
        [DeepDependency]
        IViewManager ViewManager { get; }
        #endregion

        #region Internals

        #endregion

        #endregion

        #region Constructors
        public SplashViewModel()
        {
            LoginModel.PropertyChanged += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(LoginError)) LoginError = null;
            };
        }
        #endregion

        #region Methods

        #region Command Handlers
        [CommandMethod(nameof(LoginCommand))]
        void OnLogin()
        {
            LoginModel.Password = Password.ToSimpleString();
            AccountManager.Login(LoginModel).ContinueWith((login) =>
            {
                if (!login.IsCompleted) return;

                string error = login.Result;
                if (!string.IsNullOrWhiteSpace(error)) LoginError = error;
                else ViewManager.ActiveTab = TabMenu.Home;
            });
        }
        [CommandMethod(nameof(CloseCommand))]
        void OnClose() => AccountManager.Shutdown();

        #endregion

        #endregion
    }
}
