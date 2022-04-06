using Kasi_Server.Utils.Helpers;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        public static int Value(this System.Enum instance)
        {
            return Helpers.Enum.GetValue(instance.GetType(), instance);
        }

        public static TResult Value<TResult>(this System.Enum instance)
        {
            return Conv.To<TResult>(Value(instance));
        }

        public static string Description(this System.Enum instance)
        {
            return Helpers.Enum.GetDescription(instance.GetType(), instance);
        }

        public static string GetName(this System.Enum instance)
        {
            return Helpers.Enum.GetName(instance.GetType(), instance);
        }

        public static string Join<T>(this IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            return Str.Join(list, quotes, separator);
        }

        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }

            return Regex.IsMatch(value, pattern);
        }

        public static bool IsMatch(this string value, string pattern, RegexOptions options)
        {
            if (value == null)
            {
                return false;
            }

            return Regex.IsMatch(value, pattern, options);
        }

        public static string GetMatch(this string value, string pattern)
        {
            if (value.IsEmpty())
            {
                return string.Empty;
            }

            return Regex.Match(value, pattern).Value;
        }

        public static IEnumerable<string> GetMatchingValues(this string value, string pattern)
        {
            if (value.IsEmpty())
            {
                return new string[] { };
            }

            return GetMatchingValues(value, pattern, RegexOptions.None);
        }

        public static IEnumerable<string> GetMatchingValues(this string value, string pattern, RegexOptions options)
        {
            return from Match match in GetMatches(value, pattern, options) where match.Success select match.Value;
        }

        public static MatchCollection GetMatches(this string value, string pattern, RegexOptions options)
        {
            return Regex.Matches(value, pattern, options);
        }
    }
}