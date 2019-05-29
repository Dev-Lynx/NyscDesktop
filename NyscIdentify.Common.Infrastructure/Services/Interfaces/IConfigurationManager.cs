using NyscIdentify.Common.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IConfigurationManager
    {
        #region Properties
        AppConfiguration CurrentConfiguration { get; }
        #endregion

        #region Methods
        bool Save(AppConfiguration configuration = null);
        void RestoreDefaults();
        #endregion
    }
}
