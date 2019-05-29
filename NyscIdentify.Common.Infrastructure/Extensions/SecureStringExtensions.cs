using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Extensions
{
    public static class SecureStringExtensions
    {
        public static string ToSimpleString(this SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            var value = IntPtr.Zero;
            try
            {
                value = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(value);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(value);
            }
        }

        public static SecureString ToSecureString(this string value)
        {
            if (value == null) return new SecureString();

            return value.Aggregate(new SecureString(), AppendChar, MakeReadOnly);
        }

        public static bool Compare(this SecureString ss1, SecureString ss2)
        {
            IntPtr str1 = IntPtr.Zero;
            IntPtr str2 = IntPtr.Zero;

            try
            {
                str1 = Marshal.SecureStringToBSTR(ss1);
                str2 = Marshal.SecureStringToBSTR(ss2);

                int length = Marshal.ReadInt32(str1, -4);

                // If the lengths are not equal, thier probably not equal.
                if (length != Marshal.ReadInt32(str2, -4)) return false;

                // Compare each byte of the strings.
                for (int i = 0; i < length; i++)
                    if (Marshal.ReadByte(str1, i) != Marshal.ReadByte(str2, i))
                        return false;

                return true;
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while comparing two secure strings.\n{ex}");
            }
            finally
            {
                if (str1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(str1);
                if (str2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(str2);
            }
            return false;
        }

        static SecureString MakeReadOnly(SecureString ss)
        {
            ss.MakeReadOnly();
            return ss;
        }

        static SecureString AppendChar(SecureString ss, char c)
        {
            ss.AppendChar(c);
            return ss;
        }
    }
}
