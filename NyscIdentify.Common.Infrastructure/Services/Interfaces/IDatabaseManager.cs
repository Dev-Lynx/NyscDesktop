using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IDatabaseManager
    {
        #region Properties
        IQueryable<User> Users { get; }
        IQueryable<ResourceBase> Resources { get; }
        #endregion

        #region Methods
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> UserExists(string username);

        Task<User> LoginUser(UserLoginViewModel model);
        Task<User> GetUser(string username);
        Task<User> GetUserById(string id);
        Task<bool> ValidatePassword(string id, string password);
        Task<bool> ChangePassword(string id, string currentPassword, string newPassword);

        Task<bool> AddResource(ResourceBase resource);
        Task<bool> RemoveResource(ResourceBase resource);
        Task<ResourceBase> GetResource(string id);
        Task<ResourceBase> GetResourceByUrl(string url);
        #endregion
    }
}
