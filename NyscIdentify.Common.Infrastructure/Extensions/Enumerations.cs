using NyscIdentify.Common.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Extensions
{
    public static class Enumerations
    {
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null) return string.Empty;
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return GetEnumDescription((Enum)(object)EnumValue);
        }

        public static string GetEnumColorValue(this Enum value)
        {
            if (value == null) return string.Empty;
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ColorValue[] attributes = (ColorValue[])fi.
                GetCustomAttributes(typeof(ColorValue),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Color;
            return string.Empty;
        }

        public static string ToColorValue<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return GetEnumColorValue((Enum)(object)EnumValue);
        }



        /*
        public static Enum ToEnum(this object @enum)
        {
            try
            {
                Type enumType = @enum.GetType();
                Type underlyingType = Enum.GetUnderlyingType(enumType);
                Enum enumValue = (Enum)Convert.ChangeType(@enum, underlyingType);
                return enumValue;
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while converting an object to an enum\n{ex}");
                return null;
            }
        }
        */

        public static string[] GetDescriptions<TEnum>() where TEnum : struct
        {
            Array values = Enum.GetValues(typeof(TEnum));
            string[] descriptions = new string[values.Length];

            for (int i = 0; i < values.Length; i++)
                descriptions[i] = ((TEnum)values.GetValue(i)).ToDescription();
            
            foreach (TEnum value in values)
                value.ToDescription();

            return descriptions;
        }

        public static ObservableCollection<TEnum> ToObservable<TEnum>(this TEnum @enum) where TEnum : struct
            => new ObservableCollection<TEnum>(Enum.GetValues(typeof(TEnum)).OfType<TEnum>());

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(Arr, src) - 1;
            return (j >= 0) ? Arr[j] : Arr[Arr.Length - 1];
        }
    }
}
