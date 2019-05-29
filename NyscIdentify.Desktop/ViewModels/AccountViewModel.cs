using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
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
using Fody = PropertyChanged;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class AccountViewModel : BindableBase, IWaitable
    {
        #region Properties

        #region Bindables
        [UpdateWith(Source = nameof(AccountManager), Property = nameof(IAccountManager.CurrentUser))]
        public User CurrentUser => AccountManager.CurrentUser;
        public ObservableCollection<UserRole> UserRoles { get; } = ((UserRole)0).ToObservable();

        public SecureString CurrentPassword { get; set; }
        public SecureString NewPassword { get; set; }
        public SecureString NewPasswordConfirmation { get; set; }

        public string ErrorMessage { get; set; }

        #region IWaitable Implementation

        public bool IsBusy { get; set; }
        public string BusyMessage { get; set; }
        public double Progress => 50;
        #endregion

        #endregion

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IAccountManager AccountManager { get; }
        [DeepDependency]
        IViewManager ViewManager { get; }
        [DeepDependency]
        IAlertService AlertService { get; }
        #endregion

        #region Commands
        public ICommand UpdateCommand { get; }
        #endregion

        #endregion

        #region Methods

        #region Command Handlers
        [CommandMethod(nameof(UpdateCommand))]
        void OnUpdate()
        {
            bool passwordChange = CurrentPassword.Length > 0
                || NewPassword.Length > 0
                || NewPasswordConfirmation.Length > 0;

            if (passwordChange)
            {
                string errorMessage = AccountManager.
                    ValidatePasswords(NewPassword, NewPasswordConfirmation);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ErrorMessage = errorMessage;
                    return;
                }
            }
            

            Task.Run(async () =>
            {
                IsBusy = true;

                if (passwordChange)
                {
                    BusyMessage = "Updating Security Details...";
                    bool success = await AccountManager.
                        ValidatePassword(CurrentPassword.ToSimpleString());

                    if (!success)
                    {
                        ErrorMessage = "Invalid Account Password, Please try again.";
                        IsBusy = false;
                        return;
                    }

                    success = await AccountManager.ChangeAccountPassword(CurrentPassword.
                        ToSimpleString(), NewPassword.ToSimpleString());

                    if (!success)
                    {
                        ErrorMessage = "An error occured while updating your security details. Please try again.";
                        IsBusy = false;
                        return;
                    }

                    AlertService.Success("Your account password was successfully changed");
                }
                

                BusyMessage = "Updating Account Details...";
                await AccountManager.UpdateAccount();
                AlertService.Success("Your account has successfully been updated.");

                IsBusy = false;
            });

        }
        #endregion

        [DeepInjectionMethod]
        void Initialize()
        {
            PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(NewPassword):
                    case nameof(NewPasswordConfirmation):
                        if (!string.IsNullOrWhiteSpace(ErrorMessage)) ErrorMessage = null;
                        break;
                }
            };

            ViewManager.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(IViewManager.BackstageActive):
                        if (!ViewManager.BackstageActive) return;
                        ResetPasswords();
                        break;
                }
            };

            ResetPasswords();
        }

        void ResetPasswords()
        {
            CurrentPassword = new SecureString();
            NewPassword = new SecureString();
            NewPasswordConfirmation = new SecureString();
        }

        #endregion
    }
}
