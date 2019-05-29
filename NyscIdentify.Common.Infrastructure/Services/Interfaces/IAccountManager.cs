using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IAccountManager : INotifyPropertyChanged, IWaitable
    {
        #region Properties
        User CurrentUser { get; }
        bool LoggedIn { get; }
        #endregion

        #region Methods
        Task ConductAuthentication(bool activateShell = false);
        Task<string> Login(UserLoginViewModel model);
        Task Shutdown();
        void LogOut();

        Task<bool> ValidatePassword(string password);
        string ValidatePasswords(SecureString ss1, SecureString ss2);
        Task<bool> UpdateAccount();
        Task<bool> ChangeAccountPassword(string currentPassword, string newPassword);
        #endregion
    }
}
