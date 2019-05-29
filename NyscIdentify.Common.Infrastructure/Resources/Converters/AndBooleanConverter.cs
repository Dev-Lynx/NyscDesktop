using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NyscIdentify.Common.Infrastructure.Resources.Converters
{
    public class AndBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                Visibility returnVisibility = Visibility.Visible;
                foreach (object value in values)
                {
                    if (value is bool)
                    {
                        bool currentBool = (bool)value;
                        if (!currentBool)
                        {
                            returnVisibility = Visibility.Collapsed;
                        }
                    }
                    else if (value is Visibility)
                    {
                        Visibility currentVisibility = (Visibility)value;
                        if (currentVisibility != Visibility.Visible)
                        {
                            returnVisibility = Visibility.Collapsed;
                        }
                    }
                }
                return returnVisibility;
            }
            bool boolValue = true;
            foreach (object value in values)
            {
                if (value is bool)
                {
                    boolValue = boolValue & (bool)value;
                }
                else if (value is Visibility)
                {
                    boolValue = boolValue && ((Visibility)value) == Visibility.Visible;
                }
            }
            return boolValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
