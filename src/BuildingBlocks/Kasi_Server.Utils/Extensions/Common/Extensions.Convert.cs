using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static string SafeString(this object input) => input == null ? string.Empty : input.ToString().Trim();

        public static bool ToBool(this string obj) => Conv.ToBool(obj);

        public static bool? ToBoolOrNull(this string obj) => Conv.ToBoolOrNull(obj);

        public static int ToInt(this string obj) => Conv.ToInt(obj);

        public static int? ToIntOrNull(this string obj) => Conv.ToIntOrNull(obj);

        public static short ToShort(this string obj) => Conv.ToShort(obj);

        public static short? ToShortOrNull(this string obj) => Conv.ToShortOrNull(obj);

        public static long ToLong(this string obj) => Conv.ToLong(obj);

        public static long? ToLongOrNull(this string obj) => Conv.ToLongOrNull(obj);

        public static double ToDouble(this string obj) => Conv.ToDouble(obj);

        public static double? ToDoubleOrNull(this string obj) => Conv.ToDoubleOrNull(obj);

        public static decimal ToDecimal(this string obj) => Conv.ToDecimal(obj);

        public static decimal? ToDecimalOrNull(this string obj) => Conv.ToDecimalOrNull(obj);

        public static DateTime ToDate(this string obj) => Conv.ToDate(obj);

        public static DateTime? ToDateOrNull(this string obj) => Conv.ToDateOrNull(obj);

        public static Guid ToGuid(this string obj) => Conv.ToGuid(obj);

        public static Guid? ToGuidOrNull(this string obj) => Conv.ToGuidOrNull(obj);

        public static List<Guid> ToGuidList(this string obj) => Conv.ToGuidList(obj);

        public static List<Guid> ToGuidList(this IList<string> obj) => obj == null ? new List<Guid>() : obj.Select(t => t.ToGuid()).ToList();

        public static string ToSnakeCase(this string str) => Kasi_Server.Utils.Helpers.Str.ToSnakeCase(str);

        public static string ToCamelCase(this string str) => Kasi_Server.Utils.Helpers.Str.ToCamelCase(str);
    }
}