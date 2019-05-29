using NyscIdentify.Common.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class EnumToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum)) return value;
            string s = ((Enum)value).GetEnumColorValue();

            if (string.IsNullOrWhiteSpace(s)) return Binding.DoNothing;

            return s.ToSolidBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
