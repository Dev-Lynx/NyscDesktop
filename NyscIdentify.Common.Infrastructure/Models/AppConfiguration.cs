using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models
{
    public class AppConfiguration : BindableBase
    {
        #region Properties

        #region Statics
        public static AppConfiguration DefualtConfiguration = new AppConfiguration()
        {
            ServerAddress = Core.API_BASE,
            ResourceConfiguration = new ResourceConfiguration(ResourceConfiguration.Default)
        };
        #endregion

        public string ServerAddress { get; set; } = Core.API_BASE;
        public ResourceConfiguration ResourceConfiguration { get; set; }
        #endregion

        #region Constructors
        public AppConfiguration() { }
        public AppConfiguration(AppConfiguration configuration)
        {
            ServerAddress = configuration.ServerAddress;
            ResourceConfiguration = new ResourceConfiguration(configuration.ResourceConfiguration);
        }
        #endregion
    }
}
