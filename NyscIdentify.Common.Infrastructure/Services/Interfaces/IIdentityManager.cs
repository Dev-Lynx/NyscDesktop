using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.SieveModels;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IIdentityManager : INotifyPropertyChanged
    {
        #region Properties
        User SelectedIdentity { get; set; }
        SievableCollection<User> Online { get; }
        #endregion

        #region Methods
        void Initialize();
        void Shutdown();
        Task<bool> ApproveIdentity(User identity = null);
        Task<bool> DeclineIdentity(string message, User identity = null);
        Task<bool> RevokeApprovalStatus(User identity = null);
        Task<User> GetIdentity(User identity = null);
        Task<ICollection<SimpleUserActivity>> GetActivities(User identity = null);
        #endregion
    }
}
