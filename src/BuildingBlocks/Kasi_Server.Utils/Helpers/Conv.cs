using Kasi_Server.Utils.Extensions;
using System.ComponentModel;

namespace Kasi_Server.Utils.Helpers
{
    public static class Conv
    {
        public static byte ToByte(object input) => ToByte(input, default);

        public static byte ToByte(object input, byte defaultValue) => ToByteOrNull(input) ?? defaultValue;

        public static byte? ToByteOrNull(object input)
        {
            var success = byte.TryParse(input.SafeString(), out var result);
            if (success)
                return result;
            try
            {
                var temp = ToDoubleOrNull(input, 0);
                if (temp == null)
                    return null;
                return Convert.ToByte(temp);
            }
            catch
            {
                return null;
            }
        }

        public static char ToChar(object input) => ToChar(input, default);

        public static char ToChar(object input, char defaultValue) => ToCharOrNull(input) ?? defaultValue;

        public static char? ToCharOrNull(object input)
        {
            var success = char.TryParse(input.SafeString(), out var result);
            if (success)
                return result;
            return null;
        }

        public static short ToShort(object input) => ToShort(input, default);

        public static short ToShort(object input, short defaultValue) => ToShortOrNull(input) ?? defaultValue;

        public static short? ToShortOrNull(object input)
        {
            var success = short.TryParse(input.SafeString(), out var result);
            if (success)
                return result;
            try
            {
                var temp = ToDoubleOrNull(input, 0);
                if (temp == null)
                    return null;
                return Convert.ToInt16(temp);
            }
            catch
            {
                return null;
            }
        }

        public static int ToInt(object input) => ToInt(input, default);

        public static int ToInt(object input, int defaultValue) => ToIntOrNull(input) ?? defaultValue;

        public static int? ToIntOrNull(object input)
        {
            var success = int.TryParse(input.SafeString(), out var result);
            if (success)
                return result;
            try
            {
                var temp = ToDoubleOrNull(input, 0);
                if (temp == null)
                    return null;
                return System.Convert.ToInt32(temp);
            }
            catch
            {
                return null;
            }
        }

        public static long ToLong(object input) => ToLong(input, default);

        public static long ToLong(object input, long defaultValue) => ToLongOrNull(input) ?? defaultValue;

        public static long? ToLongOrNull(object input)
        {
            var success = long.TryParse(input.SafeString(), out var result);
            if (success)
                return result;
            try
            {
                var temp = ToDecimalOrNull(input, 0);
                if (temp == null)
                    return null;
                return System.Convert.ToInt64(temp);
            }
            catch
            {
                return null;
            }
        }

        public static float ToFloat(object input, int? digits = null) => ToFloat(input, default, digits);

        public static float ToFloat(object input, float defaultValue, int? digits = null) => ToFloatOrNull(input, digits) ?? defaultValue;

        public static float? ToFloatOrNull(object input, int? digits = null)
        {
            var success = float.TryParse(input.SafeString(), out var result);
            if (!success)
                return null;
            if (digits == null)
                return result;
            return (float)Math.Round(result, digits.Value);
        }

        public static double ToDouble(object input, int? digits = null) => ToDouble(input, default, digits);

        public static double ToDouble(object input, double defaultValue, int? digits = null) => ToDoubleOrNull(input, digits) ?? defaultValue;

        public static double? ToDoubleOrNull(object input, int? digits = null)
        {
            var success = double.TryParse(input.SafeString(), out var result);
            if (!success)
                return null;
            return digits == null ? result : Math.Round(result, digits.Value);
        }

        public static decimal ToDecimal(object input, int? digits = null) => ToDecimal(input, default(decimal), digits);

        public static decimal ToDecimal(object input, decimal defaultValue, int? digits = null) => ToDecimalOrNull(input, digits) ?? defaultValue;

        public static decimal? ToDecimalOrNull(object input, int? digits = null)
        {
            var success = decimal.TryParse(input.SafeString(), out var result);
            if (!success)
                return null;
            return digits == null ? result : Math.Round(result, digits.Value);
        }

        public static bool ToBool(object input) => ToBool(input, default);

        public static bool ToBool(object input, bool defaultValue) => ToBoolOrNull(input) ?? defaultValue;

        public static bool? ToBoolOrNull(object input)
        {
            bool? value = GetBool(input);
            if (value != null)
                return value.Value;
            return bool.TryParse(input.SafeString(), out var result) ? (bool?)result : null;
        }

        private static bool? GetBool(object input)
        {
            switch (input.SafeString().ToLower())
            {
                case "0":
                case "否":
                case "不":
                case "no":
                case "fail":
                    return false;

                case "1":
                case "是":
                case "ok":
                case "yes":
                    return true;

                default:
                    return null;
            }
        }

        public static DateTime ToDate(object input, DateTime defaultValue = default) => ToDateOrNull(input) ?? DateTime.MinValue;

        public static DateTime? ToDateOrNull(object input, DateTime? defaultValue = null)
        {
            if (input == null)
                return defaultValue;
            return DateTime.TryParse(input.SafeString(), out var result) ? result : defaultValue;
        }

        public static Guid ToGuid(object input) => ToGuidOrNull(input) ?? Guid.Empty;

        public static Guid? ToGuidOrNull(object input) => Guid.TryParse(input.SafeString(), out var result) ? (Guid?)result : null;

        public static List<Guid> ToGuidList(string input) => ToList<Guid>(input);

        public static List<T> ToList<T>(string input)
        {
            var result = new List<T>();
            if (string.IsNullOrWhiteSpace(input))
                return result;
            var array = input.Split(',');
            result.AddRange(from each in array where !string.IsNullOrWhiteSpace(each) select To<T>(each));
            return result;
        }

        public static T ToEnum<T>(object input) where T : struct => ToEnum<T>(input, default);

        public static T ToEnum<T>(object input, T defaultValue) where T : struct => ToEnumOrNull<T>(input) ?? defaultValue;

        public static T? ToEnumOrNull<T>(object input) where T : struct
        {
            var success = System.Enum.TryParse(input.SafeString(), true, out T result);
            if (success)
                return result;
            return null;
        }

        public static T To<T>(object input)
        {
            if (input == null)
                return default;
            if (input is string && string.IsNullOrWhiteSpace(input.ToString()))
                return default;

            var type = Common.GetType<T>();
            var typeName = type.Name.ToLower();
            try
            {
                if (typeName == "string")
                    return (T)(object)input.ToString();
                if (typeName == "guid")
                    return (T)(object)new Guid(input.ToString());
                if (type.IsEnum)
                    return Enum.Parse<T>(input);
                if (input is IConvertible)
                    return (T)System.Convert.ChangeType(input, type);
                return (T)input;
            }
            catch
            {
                return default;
            }
        }

        internal static object ChangeType(this object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) return obj;
            else if ((underlyingType ?? type).IsEnum)
            {
                if (underlyingType != null && string.IsNullOrWhiteSpace(obj.ToString())) return null;
                else return System.Enum.Parse(underlyingType ?? type, obj.ToString());
            }
            else if (obj.GetType().Equals(typeof(DateTime)) && (underlyingType ?? type).Equals(typeof(DateTimeOffset)))
            {
                return DateTime.SpecifyKind((DateTime)obj, DateTimeKind.Local);
            }
            else if (obj.GetType().Equals(typeof(DateTimeOffset)) && (underlyingType ?? type).Equals(typeof(DateTime)))
            {
                return ((DateTimeOffset)obj).ToLocalDateTime();
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType())) return converter.ConvertFrom(obj);

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    var o = constructor.Invoke(null);
                    var propertys = type.GetProperties();
                    var oldType = obj.GetType();

                    foreach (var property in propertys)
                    {
                        var p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ChangeType(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }
    }
}