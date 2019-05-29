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
    public class CountToVisibilityConverter : IValueConverter
    {
        #region Properties
        public int HiddenCount { get; set; }
        #endregion

        #region Methods

        #region IValueConverter Implementation

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && ((int)value) > HiddenCount)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
