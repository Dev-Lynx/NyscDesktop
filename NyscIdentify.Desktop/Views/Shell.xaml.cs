using Fluent;
using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;

namespace NyscIdentify.Desktop.Views
{
    [AutoBuild]
    [AutoView(Region = Core.MAIN_REGION, View = typeof(IdentityView))]
    [AutoView(Region = Core.MAIN_REGION, View = typeof(SingleIdentityView))]
    [AutoView(Region = Core.ACCOUNT_REGION, View = typeof(AccountView))]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : RibbonWindow, IShell
    {
        public Shell()
        {
            InitializeComponent();

        }
    }
}
