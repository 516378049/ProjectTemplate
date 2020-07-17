using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Common
{
    public static class Conversion
    {
        private static MethodInfo _stringToEnumMethod;

        ///<summary>将指定类型转换为字符串类型</summary>
        public static string ToString<T>(T obj)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return obj.ToString();
            }
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if ((converter != null) && (converter.CanConvertTo(typeof(string))))
            {
                return converter.ConvertToInvariantString(obj);
            }
            return null;
        }

        ///<summary>将byte数组转换成hex字符串</summary>
        public static string Bytes2HexString(byte[] s)
        {
            if (s == null)
            {
                return "";
            }

            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                res += s[i].ToString("X2");
            }

            return res;
        }

        ///<summary>将hex字符串转换成byte数组</summary>
        public static byte[] HexString2Bytes(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString)) return null;
            byte[] res = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                res[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return res;
        }

        ///<summary>将字符串类型转换为指定类型</summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "TypeConverter throws System.Exception instead of a more specific one.")]
        public static bool TryFromString(Type type, string value, out object result)
        {
            result = null;
            if (type == typeof(string))
            {
                result = value;
                return true;
            }
            if (type.IsEnum)
            {
                return TryFromStringToEnumHelper(type, value, out result);
            }
            if (type == typeof(Color))
            {
                Color color;
                bool rval = TryFromStringToColor(value, out color);
                result = color;
                return rval;
            }
            // TypeConverter doesn't really have TryConvert APIs.  We should avoid TypeConverter.IsValid
            // which performs a duplicate conversion, and just handle the general exception ourselves.
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if ((converter != null) && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                    return true;
                }
                catch
                {
                    // Do nothing
                }
            }
            return false;
        }

        ///<summary>将字符串类型转换为指定的枚举类型</summary>
        public static bool TryFromStringToEnum<T>(string value, out T result) where T : struct
        {
            return Enum.TryParse(value, ignoreCase: true, result: out result);
        }

        private static bool TryFromStringToEnumHelper(Type enumType, string value, out object result)
        {
            result = null;
            if (_stringToEnumMethod == null)
            {
                _stringToEnumMethod = typeof(Conversion).GetMethod("TryFromStringToEnum",
                                                                       BindingFlags.Static | BindingFlags.NonPublic);
                Debug.Assert(_stringToEnumMethod != null);
            }
            var args = new object[] { value, null };
            var rval = (bool)_stringToEnumMethod.MakeGenericMethod(enumType).Invoke(null, args);
            result = args[1];
            return rval;
        }

        ///<summary>将字符串的字体名称转换为字体类型</summary>
        public static bool TryFromStringToFontFamily(string fontFamily, out FontFamily result)
        {
            result = null;
            bool converted = false;
            foreach (FontFamily fontFamilyTemp in FontFamily.Families)
            {
                if (fontFamily.Equals(fontFamilyTemp.Name, StringComparison.OrdinalIgnoreCase))
                {
                    result = fontFamilyTemp;
                    converted = true;
                    break;
                }
            }
            return converted;
        }

        ///<summary>将字符串的颜色转换为颜色类型</summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "TypeConverter throws System.Exception instad of a more specific one.")]
        public static bool TryFromStringToColor(string value, out Color result)
        {
            result = default(Color);

            // Parse color specified as hex number
            if (value.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                // Only allow colors in form of #RRGGBB or #RGB
                if ((value.Length != 7) && (value.Length != 4))
                {
                    return false;
                }

                // Expand short version
                if (value.Length == 4)
                {
                    char[] newValue = new char[7];
                    newValue[0] = '#';
                    newValue[1] = newValue[2] = value[1];
                    newValue[3] = newValue[4] = value[2];
                    newValue[5] = newValue[6] = value[3];
                    value = new string(newValue);
                }
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
            Debug.Assert((converter != null) && (converter.CanConvertFrom(typeof(string))));

            // There are no TryConvert APIs on TypeConverter so we have to catch exception. 
            // In addition to that, invalid conversion just throws System.Exception with misleading message,
            // instead of a more specific exception type. 
            try
            {
                result = (Color)converter.ConvertFromInvariantString(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        ///<summary>获取图片类型</summary>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "Format names are used in Http headers and are usually specified in lower case")]
        public static string NormalizeImageFormat(string value)
        {
            value = value.ToLowerInvariant();
            switch (value)
            {
                case "jpeg":
                case "jpg":
                case "pjpeg":
                    return "jpeg";

                case "png":
                case "x-png":
                    return "png";

                case "icon":
                case "ico":
                    return "icon";
            }
            return value;
        }

        ///<summary>将字符串的图片格式转换为图片格式类型</summary>
        public static bool TryFromStringToImageFormat(string value, out ImageFormat result)
        {
            result = default(ImageFormat);

            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            if (value.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring("image/".Length);
            }
            value = NormalizeImageFormat(value);

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(ImageFormat));
            Debug.Assert((converter != null) && (converter.CanConvertFrom(typeof(string))));

            try
            {
                result = (ImageFormat)converter.ConvertFromInvariantString(value);
            }
            catch (NotSupportedException)
            {
                return false;
            }

            return true;
        }
    }
}
