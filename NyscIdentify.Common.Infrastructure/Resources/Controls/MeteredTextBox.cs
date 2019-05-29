using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class MeteredTextBox : TextBox
    {
        #region Properties

        #region Dependency Properties


        public int TextLimit
        {
            get { return (int)GetValue(TextLimitProperty); }
            set { SetValue(TextLimitProperty, value); }
        }

        public static readonly DependencyProperty TextLimitProperty =
            DependencyProperty.Register("TextLimit", typeof(int), 
                typeof(MeteredTextBox), new PropertyMetadata(int.MaxValue));


        #endregion

        #endregion

        #region Methods

        #region Overrides
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            //this.len
            base.OnPreviewTextInput(e);
        }
        #endregion

        #endregion
    }
}
