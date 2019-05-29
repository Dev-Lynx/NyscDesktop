using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services.Interfaces
{
    public interface IViewManager : INotifyPropertyChanged
    {
        bool BackstageActive { get; set; }
        TabMenu ActiveTab { get; set; }
    }
}
