using NyscIdentify.Common.Infrastructure.Models;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IResourceManager
    {
        #region Properties
        ResourceConfiguration Configuration { get; }
        #endregion

        #region Methods
        Task ResolveResource(IResource resource, bool highPriority = false);
        #endregion
    }
}
