using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class EnumToBoolConverter : IValueConverter
    {
        #region Properties
        public bool Inverse { get; set; }
        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object trueValue, System.Globalization.CultureInfo culture)
        {
            bool result = false;
            if (value != null && value.GetType().IsEnum)
                result = (Enum.Equals(value, trueValue));

            if (Inverse) result = !result;

            if (targetType == typeof(Visibility))
                return result ? Visibility.Visible : Visibility.Collapsed;


            if (value != null && value.GetType().IsEnum)
                return result;
            else
                return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object originalValue, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return originalValue;
            else
                return Binding.DoNothing;
        }

        #endregion
    }
}
