using JWT;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.SieveModels;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Unity;
using RestSharp.Authenticators;
using RestSharp.Serializers.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace NyscIdentify.Common.Infrastructure.Services
{
    [AutoBuild]
    public class AccountManager : BindableBase, IAccountManager
    {
        #region Properties
        public User CurrentUser { get; private set; }
        public bool LoggedIn { get; private set; }

        #region IWaitable Implementation
        public bool IsBusy { get; set; }
        public string BusyMessage { get; set; }
        public double Progress { get; set; } = 50.0;
        #endregion

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IConnectionService ConnectionService { get; }
        [DeepDependency]
        IDatabaseManager DatabaseManager { get; }
        [DeepDependency]
        IUnityContainer Container { get; }
        [DeepDependency]
        IIdentityManager IdentityManager { get; }
        [DeepDependency]
        IAlertService AlertService { get; }
        #endregion

        #region Internals
        RestSharp.RestClient Client { get; } = new RestSharp.RestClient();
        JsonSerializer Serializer { get; } = new JsonSerializer();
        JwtSecurityTokenHandler TokenHandler { get; } = new JwtSecurityTokenHandler();

        IAuthenticator Authenticator
        {
            get => Client.Authenticator;
            set => Client.Authenticator = value;
        }

        ISplash Splash { get; set; }
        #endregion

        #endregion

        #region Methods

        #region IAccountManager Implementation
        public async Task ConductAuthentication(bool activateShell = false)
        {
            var shell = Container.Resolve<IShell>();
            shell.Hide();

            while (shell.IsVisible)
                await Task.Delay(200);

            Splash = Container.Resolve<ISplash>();
            Splash.ShowDialog();

            if (activateShell) shell.Show();
        }

        /// <summary>
        /// Login to the application
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The error that occured during the login. 
        /// If the returned value was empty, no error occured.
        /// </returns>
        public async Task<string> Login(UserLoginViewModel model)
        {
            IsBusy = true;
            string result = await InternalUserLogin(model);
            IsBusy = false;
            LoggedIn = string.IsNullOrWhiteSpace(result);

            if (LoggedIn)
            {
                Splash.Close();
                Splash = null;
                Container.RegisterInstance<IAuthenticator>(new 
                    JwtAuthenticator(CurrentUser.AccessToken));


                IdentityManager.Initialize();
            }

            return result;
        }

        public Task Shutdown()
        {
            if (Splash != null) Splash.Close();
            Application.Current.Shutdown();
            return Task.CompletedTask;
        }

        public void LogOut()
        {
            CurrentUser = new User();
            LoggedIn = false;
            IdentityManager.Shutdown();
            Core.Dispatcher.Invoke(() => ConductAuthentication(true));
        }

        public Task<bool> ValidatePassword(string password) => DatabaseManager.
            ValidatePassword(CurrentUser.Id, password);

        public string ValidatePasswords(SecureString ss1, SecureString ss2)
        {
            if (ss1.Length == 0 && ss2.Length == 0)
                return string.Empty;

            if (ss1.Length < 8 || ss2.Length < 8)
                return "Passwords must be 8 characters or more";

            if (!ss1.Compare(ss2)) return "The passwords do not match. Please try again";

            return string.Empty;
        }

        public async Task<bool> UpdateAccount()
        {
            User user = CurrentUser;
            RestSharp.IRestResponse response = await Client.MakeRequest(Core.API_UPDATE_ACCOUNT, 
                user, RestSharp.Method.POST);

            // if (!response.IsSuccessful)
                
            
            bool success = false;
            if (success = await DatabaseManager.UpdateUser(user))
                CurrentUser = await DatabaseManager.GetUserById(user.Id);
            return success;
        }

        public async Task<bool> ChangeAccountPassword(string currentPassword, string newPassword)
        {
            User user = CurrentUser;

            var body = new
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            RestSharp.IRestResponse response = await Client.MakeRequest(Core.API_CHANGE_PASSWORD,
                body, RestSharp.Method.POST);

            if (!response.IsSuccessful) return false;

            bool success = await DatabaseManager
                .ChangePassword(user.Id, currentPassword, newPassword);

            return success;
        }
        #endregion

        #region Requests
        async Task<User> GetCurrentUser(string token)
        {
            return await Client.Get<User>(Core.API_BASE + "account/user");
        }

        async Task<string> GetAccessToken(UserLoginViewModel model)
        {
            RestSharp.IRestResponse response = null;
            try
            {
                RestSharp.IRestRequest request = new RestSharp.Serializers.Newtonsoft.Json.RestRequest(Core.API_BASE + "auth/login");
                request.JsonSerializer = new NewtonsoftJsonSerializer(Serializer);
                request.AddJsonBody(model);

                response = await Client.ExecutePostTaskAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return "Invalid Credentials. Try again";

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return "An error occured. Are you sure you are on the right role?";

                dynamic token = JsonConvert.DeserializeObject(response.Content);
                string accessToken = token.access_token;

                JwtSecurityToken jwtToken = null;
                try
                {
                    jwtToken = TokenHandler.ReadJwtToken(accessToken);
                }
                catch (Exception ex)
                {
                    Logger.Error($"A JWT validation error occured.\n{ex}");
                    return "An account error has occured."
                        + " Please visit our website if you keep getting this message.";
                }

                var verified = jwtToken.Claims.SingleOrDefault(c => c.Type == "ver");

                bool.TryParse(verified.Value, out bool isVerified);

                if (!isVerified)
                    return "Your account is unverified." +
                        " Please visit our the website to verify your account.";

                return accessToken;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while attempting to login to the server. {ex}");
            }

            return "An unexpected error occured. Please try again.";
        }

        async Task<bool> LoginLocally(UserLoginViewModel model)
        {
            try
            {
                User user = await DatabaseManager.LoginUser(model);
                if (user == null) return false;

                CurrentUser = user;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while attempting a local login.\n{ex}");
            }
            return false;
        }

        async Task<string> InternalUserLogin(UserLoginViewModel model)
        {
            string error = string.Empty;
            BusyMessage = "Logging In...";

            // Attempt to locally login
            if (await LoginLocally(model)) return error;

            // Connect to the API to get a token for the user
            string token = await GetAccessToken(model);

            // Make sure the result was not an error message
            if (!TokenHandler.CanReadToken(token)) return token;

            Authenticator = new JwtAuthenticator(token);


            // Attempt to get the user's data
            BusyMessage = "Accessing Details...";
            User user = await GetCurrentUser(token);

            if (user == null) return "An unexpected error occured. Please try again.";

            user.Password = model.Password;
            user.AccessToken = token;

            // Save user details in the database
            BusyMessage = "Almost Done...";
            if (await DatabaseManager.AddUser(user))
            {
                CurrentUser = await DatabaseManager.GetUser(user.Username);
                return error;
            }

            return "An unexpected error occured. Please try again.";
        }

        
        #endregion



        #endregion
    }
}
