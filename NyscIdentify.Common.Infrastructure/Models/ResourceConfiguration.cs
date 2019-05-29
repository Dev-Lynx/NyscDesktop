using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models
{
    public class ResourceConfiguration : BindableBase
    {
        #region Properties

        #region Statics
        public static ResourceConfiguration Default = new ResourceConfiguration()
        {
            LifeSpan = TimeSpan.FromDays(30),
            Location = Core.RESOURCE_DIR,
            SizeLimit = 3 * 1024,
            TempLocation = Core.TEMP_DIR
        };
        #endregion

        public TimeSpan LifeSpan { get; set; }
        public string Location { get; set; }
        public float SizeLimit { get; set; }
        public string TempLocation { get; set; }
        #endregion

        #region Constructors
        public ResourceConfiguration() { }
        public ResourceConfiguration(ResourceConfiguration configuration)
        {
            LifeSpan = configuration.LifeSpan;
            Location = configuration.Location;
            SizeLimit = configuration.SizeLimit;
        }
        #endregion
    }
}
