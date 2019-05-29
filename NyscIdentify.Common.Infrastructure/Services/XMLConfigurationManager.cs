using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NyscIdentify.Common.Infrastructure.Services
{
    [AutoBuild]
    public class XMLConfigurationManager : BindableBase, IConfigurationManager
    {
        #region Properties
        public AppConfiguration CurrentConfiguration { get; private set; }

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        #endregion

        #endregion

        #region Methods

        #region IConfigurationManager Implementation
        public bool Save(AppConfiguration configuration = null)
        {
            if (configuration == null)
                configuration = CurrentConfiguration;

            try
            {
                using (var stream = new StreamWriter(Core.CONFIGURATION_PATH))
                    new XmlSerializer(typeof(AppConfiguration)).Serialize(stream, configuration);

                Logger.Debug("Configuration has been successfully initialized");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while attempting to save configuration. \n {0}", ex);
                return false;
            }
        }

        public void RestoreDefaults() => CurrentConfiguration = new AppConfiguration(AppConfiguration.DefualtConfiguration);
        #endregion

        void LoadConfiguration()
        {
            AppConfiguration configuration = null;
            try
            {
                using (var stream = new FileStream(Core.CONFIGURATION_PATH, FileMode.Open, FileAccess.Read))
                    configuration = (AppConfiguration)new 
                        XmlSerializer(typeof(AppConfiguration)).Deserialize(stream);

                CurrentConfiguration = new AppConfiguration(configuration);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while attempting to load configuration...\n{0}", ex);
                CurrentConfiguration = new AppConfiguration(AppConfiguration.DefualtConfiguration);
                Save();
            }
        }

        [DeepInjectionMethod]
        void Initialize()
        {
            LoadConfiguration();
        }
        #endregion
    }
}
