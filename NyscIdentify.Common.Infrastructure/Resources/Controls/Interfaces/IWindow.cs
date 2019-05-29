using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces
{
    public interface IWindow : IView
    {
        bool IsVisible { get; }
        WindowState WindowState { get; set; }
        void Show();
        void Hide();
        bool? ShowDialog();
        void DragMove();
        void Close();
    }
}
