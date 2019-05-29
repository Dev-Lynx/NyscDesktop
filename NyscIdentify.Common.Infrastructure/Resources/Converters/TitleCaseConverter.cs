using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class TitleCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            return ToTitleCase(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        //---------------------------------------------------------------
        // Get title case of a string (every word with leading upper case,
        //                             the rest is lower case)
        //    i.e: ABCD EFG -> Abcd Efg,
        //         john doe -> John Doe,
        //         miXEd CaSING - > Mixed Casing
        //---------------------------------------------------------------
        public static string ToTitleCase(string str)
        {
            return CultureInfo.InvariantCulture.
                TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
