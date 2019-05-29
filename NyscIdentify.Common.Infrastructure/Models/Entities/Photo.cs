using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.Entities
{
    public enum PhotoType
    {
        [Description("Any")]
        Any,
        [Description("Passport")]
        Passport,
        [Description("Signature")]
        Signature
    }

    public class Photo : ResourceBase
    {
        #region Properties
        public PhotoType Type { get; set; }
        #endregion
    }
}
