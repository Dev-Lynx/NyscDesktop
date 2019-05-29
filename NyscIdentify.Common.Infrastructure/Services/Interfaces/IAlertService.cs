using NyscIdentify.Common.Infrastructure.Resources.Controls.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IAlertService : INotifyPropertyChanged
    {
        AlertContext Context { get; }

        void Success(string message, TimeSpan? duration = null, bool closable = true);
        void Information(string message, TimeSpan? duration = null, bool closable = true);
        void Warning(string message, TimeSpan? duration = null, bool closable = true);
        void Error(string message, TimeSpan? duration = null, bool closable = true);
    }
}
