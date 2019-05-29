using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IConnectionService
    {
        event EventHandler ConnectionEstablished;
        event EventHandler ConnectionLost;

        bool WatchConnection { get; }

        bool IsActive { get; }
        bool IsConnected { get; }
        bool CheckForConnection();
        void Initialize();
    }
}
