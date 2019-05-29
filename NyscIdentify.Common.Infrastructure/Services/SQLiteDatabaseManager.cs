using NyscIdentify.Common.Infrastructure.Data;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Attributes;
using Fody = PropertyChanged;

namespace NyscIdentify.Common.Infrastructure.Services
{
    [AutoBuild]
    public class SQLiteDatabaseManager : IDatabaseManager
    {
        #region Properties

        public IQueryable<User> Users => Context.Users.AsQueryable();
        public IQueryable<ResourceBase> Resources => Context.Resources.AsQueryable();

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        Context Context { get; }
        #endregion

        #region Internals
        SemaphoreSlim Throttler { get; } = new SemaphoreSlim(0);
        #endregion

        #endregion

        #region Methods

        #region IDatabaseManager Implementation
        public async Task<bool> AddUser(User user)
        {
            user.Password = Hasher.Hash(user.Password);
            await Context.Users.AddAsync(user);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePassword(string id, string currentPassword, string newPassword)
        {
            User user = await GetUserById(id);

            bool valid = await ValidatePassword(id, currentPassword);
            if (!valid) return false;

            user.Password = Hasher.Hash(newPassword);
            await UpdateUser(user);
            return true;
        }

        public async Task<bool> ValidatePassword(string id, string password)
        {
            User user = await GetUserById(id);
            return Hasher.Verify(password, user.Password);
        }

        public async Task<bool> UpdateUser(User user)
        {
            await RunTask(() => Context.Users.Update(user));
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<User> LoginUser(UserLoginViewModel model)
        {
            try
            {
                User user = await GetUser(model.Username);
                if (user == null) return null;
                if (!Hasher.Verify(model.Password, user.Password)) return null;
                return user;
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while attempting login.\n{ex}");
            }

            return null;
        }

        public async Task<User> GetUserById(string id)
        {
            return await RunTask(() => Users.
                SingleOrDefault(u => u.Id == id));
        }

        public async Task<User> GetUser(string username)
        {
            return await RunTask(() => Users.
                SingleOrDefault(u => u.Username == username));
        }


        public async Task<bool> UserExists(string username)
        {
            return await RunTask(() => Users.Any(u => u.Username == username));
        }

        public async Task<ResourceBase> GetResource(string id)
        {
            return await RunTask(() => Resources.FirstOrDefault(r => r.Id == id));
        }

        public async Task<ResourceBase> GetResourceByUrl(string url)
        {
            return await RunTask(() => Resources.FirstOrDefault(r => r.Url == url));
        }

        public async Task<bool> AddResource(ResourceBase resource)
        {
            await RunTask(() => Context.Resources.Add(resource));
            return await RunTask(() => Context.SaveChanges() > 0);
        }

        public async Task<bool> RemoveResource(ResourceBase resource)
        {
            await RunTask(() => Context.Resources.Remove(resource));
            return await RunTask(() => Context.SaveChanges() > 0);
        }
        #endregion

        [DeepInjectionMethod]
        void Initialize()
        {
            Throttler.Release(1);
        }

        async Task<TResult> RunTask<TResult>(Func<TResult> func)
        {
            await Throttler.WaitAsync();
            var result = await Task.Run(func);
            Throttler.Release(1);
            return result;
        }
        #endregion
    }
}
