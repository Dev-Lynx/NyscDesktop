using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class NullableToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }
        public Visibility NullValue { get; set; } = Visibility.Collapsed;
        public Visibility NotNullValue { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool predicate = value == null;
            if (Inverse) predicate = !predicate;
            return predicate ? NullValue : NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
