using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure
{
    public enum Layout
    {
        [Description("List Layout")]
        List,
        [Description("Grid Layout")]
        Grid/*,
        [Description("Card Layout")]
        Card
            */
    }

    public enum RequestStatus
    {
        Pending,
        Success, 
        Failed,
    }

    public enum TabMenu
    {
        Home, 
        [Description("Single View")]
        SingleView
    }
}
