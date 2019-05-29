using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;
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

namespace NyscIdentify.Desktop.Views
{
    [AutoView(Region = Core.IDENTITY_TIMELINE_REGION, View = typeof(Views.IdentityTimeline))]
    public partial class SingleIdentityView : UserControl, IView
    {
        public SingleIdentityView()
        {
            InitializeComponent();
        }
    }
}
