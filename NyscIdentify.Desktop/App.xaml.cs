using CommonServiceLocator;
using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Data;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;
using NyscIdentify.Common.Infrastructure.Services;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using NyscIdentify.Desktop.Services;
using NyscIdentify.Desktop.Views;
using Prism.Ioc;
using Prism.Logging;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace NyscIdentify.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region Properties

        #region Internals
        IAccountManager AccountManager { get; set; }
        IShell Shell { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Overrides
        protected override void OnStartup(StartupEventArgs e)
        {
            Core.Initialize();
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            Shell = ServiceLocator.Current.TryResolve<IShell>();

            AccountManager = ServiceLocator.Current.TryResolve<IAccountManager>();
            AccountManager.ConductAuthentication().Wait();

            if (!AccountManager.LoggedIn)
            {
                AccountManager.Shutdown();
                return null;
            }

            return (Window)Shell;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            IUnityContainer container = containerRegistry.GetContainer();
            container.AddNewExtension<AutomaticBuildExtension>();
            container.AddNewExtension<DeepDependencyExtension>();
            container.AddNewExtension<DeepMethodExtension>();
            container.AddNewExtension<AutomaticPropertyUpdateExtension>();
            container.AddNewExtension<AutomaticCommandExtension>();

            container.AddNewExtension<BuildTrackingExtension>();
            container.AddNewExtension<AutomaticViewExtension>();
            

            containerRegistry.RegisterSingleton<ILoggerFacade, NLogger>();
            containerRegistry.RegisterSingleton<IConfigurationManager, XMLConfigurationManager>();
            containerRegistry.RegisterSingleton<IAlertService, AlertService>();
            containerRegistry.RegisterSingleton<IResourceManager, LocalResourceManager>();
            containerRegistry.RegisterSingleton<IConnectionService, ConnectionService>();
            containerRegistry.RegisterSingleton<IDatabaseManager, SQLiteDatabaseManager>();
            containerRegistry.RegisterSingleton<IAccountManager, AccountManager>();
            containerRegistry.RegisterSingleton<IIdentityManager, IdentityManager>();
            containerRegistry.RegisterSingleton<IViewManager, FluentViewManager>();

            container.RegisterSingleton<Context>();
            containerRegistry.RegisterSingleton<IShell, Shell>();
            containerRegistry.Register<ISplash, Splash>();
        }
        #endregion

        #endregion
    }
}
