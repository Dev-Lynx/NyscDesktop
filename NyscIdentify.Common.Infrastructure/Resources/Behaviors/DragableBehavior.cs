using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NyscIdentify.Common.Infrastructure.Resources.Behaviors
{
    public class DragableBehavior : Behavior<Window>
    {
        #region Properties
        MouseButtonEventHandler MouseEventHandler;
        #endregion

        #region Methods

        #region Overrides
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDown += MouseEventHandler = (s, e) =>
            {
                try { AssociatedObject.DragMove(); }
                catch { }
            };
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseDown -= MouseEventHandler;
        }
        #endregion

        #endregion
    }
}
