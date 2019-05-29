using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IWaitable
    {
        bool IsBusy { get; }
        string BusyMessage { get; }
        double Progress { get; }
    }

    public interface IRequestStatusAware
    {
        RequestStatus Status { get; set; }
    }
}
