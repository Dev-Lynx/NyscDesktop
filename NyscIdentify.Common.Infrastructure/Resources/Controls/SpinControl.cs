using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class SpinControl : ContentControl
    {
        #region Properties

        #region Dependency Properties

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(SpinControl), new PropertyMetadata(false));

        #endregion

        #endregion

        #region Methods
        #endregion
    }
}
