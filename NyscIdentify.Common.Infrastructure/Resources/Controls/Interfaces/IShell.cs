using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces
{
    public interface IShell : IWindow { }

    public interface ISplash : IWindow { }

    public interface IView
    {
        bool IsLoaded { get; }
        event RoutedEventHandler Loaded;
    }
}
