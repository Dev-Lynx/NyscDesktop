using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class VisibleWhenZeroConverter : IValueConverter
    {
        #region Properties
        public bool Inverse { get; set; }
        public Visibility InvisibiltyType { get; set; } = Visibility.Hidden;
        #endregion  

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value is int n) result = n == 0;//  && !Inverse ? Visibility.Visible : Visibility.Hidden;
            if (value is double d) result = d == .0;// && !Inverse ? Visibility.Visible : Visibility.Hidden;

            if (Inverse) result = !result;

            var visibility = result ? Visibility.Visible : InvisibiltyType;

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
