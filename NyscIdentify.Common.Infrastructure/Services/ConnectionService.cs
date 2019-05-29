using NETWORKLIST;
using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NyscIdentify.Common.Infrastructure.Services
{
    public class ConnectionService : BindableBase, IConnectionService, INetworkListManagerEvents
    {
        #region Properties

        #region Constants
        const double OFFLINE_RELOAD_INTERVAL = 15000;
        const double ONLINE_WATCH_INTERVAL = 30000;
        #endregion

        #region Events
        public event EventHandler ConnectionEstablished;
        public event EventHandler ConnectionLost;
        #endregion

        #region Bindables
        bool _isActive = false;
        public bool IsActive { get => _isActive; private set => SetProperty(ref _isActive, value); }
        bool _isConnected = false;
        public bool IsConnected { get => _isConnected; private set => SetProperty(ref _isConnected, value); }

        bool _watchConnection = true;
        /// <summary>
        /// Decides whether this service should keep checking for internet connection.
        /// </summary>
        public bool WatchConnection
        {
            get => _watchConnection;
            set
            {
                bool wasWatching = _watchConnection;
                SetProperty(ref _watchConnection, value);

                if (!wasWatching && value) StartTimer(OFFLINE_RELOAD_INTERVAL);
                else if (wasWatching && !value) StopTimer();
            }
        }
        #endregion

        #region Internals
        IConnectionPoint ConnectionPoint;
        INetworkListManager NetworkManager { get; set; }
        WebClient Client { get; set; }
        Timer ConnectionTimer { get; set; } = new Timer() { AutoReset = true };
        ElapsedEventHandler OnTimerElapsed;

        const string TestSite = "http://clients3.google.com/generate_204";
        int Cookie;
        Guid Guid;
        #endregion

        #region Services
        ILoggerFacade Logger { get; }
        #endregion


        #endregion

        #region Constructors
        public ConnectionService(ILoggerFacade logger)
        {
            NetworkManager = new NetworkListManager();
            Logger = logger;

            OnTimerElapsed = (s, e) => CheckForConnection();

            ConnectionEstablished += (s, e) =>
            {
                Logger.Debug("Internet Connection has been established.");
                StartTimer(ONLINE_WATCH_INTERVAL);
            };

            ConnectionLost += (s, e) =>
            {
                Logger.Debug("Internet Connection has been lost.");
                StartTimer(OFFLINE_RELOAD_INTERVAL);
            };


            Initialize();
        }
        #endregion

        #region Methods

        #region Imports
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        #endregion

        #region INetworkListManagerEvents
        public void ConnectivityChanged(NLM_CONNECTIVITY connectivity)
        {
            if (!WatchConnection) return;

            switch (connectivity)
            {
                case NLM_CONNECTIVITY.NLM_CONNECTIVITY_DISCONNECTED:
                    ConnectionLost?.Invoke(this, EventArgs.Empty);
                    break;

                default:
                    CheckForConnection();
                    break;
            }
        }
        #endregion

        #region IConnectionService Implementation
        public void Initialize()
        {
            if (IsActive) return;

            StartTimer(OFFLINE_RELOAD_INTERVAL);
            try
            {
                Guid = typeof(INetworkListManagerEvents).GUID;
                ((IConnectionPointContainer)NetworkManager).FindConnectionPoint(ref Guid, out ConnectionPoint);
                ConnectionPoint.Advise(this, out Cookie);
            }
            catch (Exception e) { Logger.Error("{0}", e); Logger.Debug("The connection service failed to initialize."); return; }

            if (!CheckForConnection()) ConnectionLost(this, EventArgs.Empty);

            IsActive = true;
            Logger.Debug("The Connection Service has successfully been initialized.");
        }

        public bool CheckForConnection()
        {
            bool wasConnected = IsConnected;

            using (Client = new WebClient())
                try { Client.OpenRead(TestSite); IsConnected = true; }
                catch
                {
                    IsConnected = false;
                    if (wasConnected && !IsConnected)
                        ConnectionLost?.Invoke(this, EventArgs.Empty);
                }

            if (!wasConnected && IsConnected)
                ConnectionEstablished?.Invoke(this, EventArgs.Empty);
            return IsConnected;
        }

        void StartTimer(double interval)
        {
            StopTimer();
            ConnectionTimer = new Timer(interval) { AutoReset = true };
            ConnectionTimer.Elapsed += OnTimerElapsed;
            ConnectionTimer.Start();
        }

        void StopTimer() => ConnectionTimer?.Dispose();
        #endregion

        #endregion
    }
}
