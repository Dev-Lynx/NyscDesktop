using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NyscIdentify.Common.Infrastructure.Resources.AttachedProperties
{
    public static class Directions
    {
        #region Properties

        public static double GetHorizontal(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalProperty);
        }

        public static void SetHorizontal(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalProperty, value);
        }

        public static readonly DependencyProperty HorizontalProperty =
            DependencyProperty.RegisterAttached("Horizontal", typeof(double), typeof(Directions), new PropertyMetadata(.0));




        public static double GetVertical(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalProperty);
        }

        public static void SetVertical(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalProperty, value);
        }

        public static readonly DependencyProperty VerticalProperty =
            DependencyProperty.RegisterAttached("Vertical", typeof(double), typeof(Directions), new PropertyMetadata(.0));


        #endregion
    }
}
