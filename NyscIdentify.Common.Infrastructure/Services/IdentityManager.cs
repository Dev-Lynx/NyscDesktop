using Newtonsoft.Json;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.SieveModels;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace NyscIdentify.Common.Infrastructure.Services
{
    [AutoBuild]
    public class IdentityManager : BindableBase, IIdentityManager
    {
        #region Properties

        #region Bindables
        public User SelectedIdentity { get; set; }
        public SievableCollection<User> Online { get; private set; }
        #endregion

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IAlertService AlertService { get; }
        [DeepDependency]
        IUnityContainer Container { get; }
        IAuthenticator Authenticator { get => Client.Authenticator; set { Client.Authenticator = value; } }
        #endregion

        #region Internals
        IRestClient Client { get; } = new RestClient();
        #endregion

        #endregion

        #region Methods
        public async Task<bool> ApproveIdentity(User identity = null)
        {
            if (identity == null) identity = SelectedIdentity;
            if (identity == null) return false;

            IRestResponse response = await Client.MakeRequest(Core.API_APPROVE_IDENTITY, 
                identity.Id, Method.POST);

            if (response.IsSuccessful)
                AlertService.Success($"{identity.FullName} was successfully approved.");
            else
                AlertService.Error($"An error occured while approving {identity.FullName}. " +
                    $"Please try again.");
            return response.IsSuccessful;
        }

        public async Task<bool> DeclineIdentity(string message, User identity = null)
        {
            if (identity == null) identity = SelectedIdentity;
            if (identity == null) return false;

            var body = new
            {
                Identity = identity.Id,
                Message = message
            };

            IRestResponse response = await Client.MakeRequest(Core.API_DECLINE_IDENTITY,
                body, Method.POST);

            if (response.IsSuccessful)
                AlertService.Success($"{identity.FullName} was successfully declined.");
            else AlertService.Error($"An error occured while declining {identity.FullName}." +
                $"Please try again.");
            return response.IsSuccessful;
        }

        public async Task<bool> RevokeApprovalStatus(User identity = null)
        {
            if (identity == null) identity = SelectedIdentity;
            if (identity == null) return false;

            IRestResponse response = await Client.MakeRequest(Core.API_REVOKE_IDENTITY_EVALUATION,
                identity.Id, Method.POST);

            if (response.IsSuccessful)
                AlertService.Success($"Approval of {identity.FullName} was successfully revoked.");
            else
                AlertService.Error($"An error occured while revoking the " +
                    $"approval status of {identity.FullName}. " +
                    $"Please try again.");
            return response.IsSuccessful;
        }

        public async Task<User> GetIdentity(User identity = null)
        {
            if (identity == null) identity = SelectedIdentity;
            if (identity == null) return identity;

            var queryParameters = new (string Name, string Value)[]
            {
                ("id", identity.Id)
            };

            identity = await Client.Get<User>(Core.API_GET_SINGLE_IDENTITY, queryParameters);

            if (identity == null)
                AlertService.Error($"An error occured while accessing {SelectedIdentity.FullName} " +
                    $"on the server. Please try again later.");

            return identity;
        }

        public async Task<ICollection<SimpleUserActivity>> GetActivities(User identity = null)
        {
            if (identity == null) identity = SelectedIdentity;
            if (identity == null) return null;

            var queryParameters = new (string Name, string Value)[]
            {
                ("id", identity.Id)
            };

            return await Client.Get<List<SimpleUserActivity>>
                (Core.API_GET_ACTIVITIES, queryParameters);
        }

        public void Initialize()
        {
            Authenticator = Container.Resolve<IAuthenticator>();
            Online = new SievableCollection<User>(Core.API_GET_IDENTITIES);
        }

        public void Shutdown()
        {
            Authenticator = null;
            Online = null;
        }
        #endregion
    }
}
