using NyscIdentify.Common.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class IdentityRibbon : Fluent.Ribbon
    {
        #region Properties
        RoutedEventHandler LoadedHandler;

        #region Elements
        Fluent.ToggleButton MinimizeButton { get; set; }
        #endregion

        #endregion

        #region Constructors
        public IdentityRibbon()
        {
            Loaded += LoadedHandler = (s, e) =>
            {
                MinimizeButton = this.FindChild<Fluent.ToggleButton>
                ("PART_MinimizeButton") as Fluent.ToggleButton;

                MinimizeButton.FindParent<Grid>().Visibility 
                = Visibility.Hidden;
            };
        }
        #endregion
    }
}
