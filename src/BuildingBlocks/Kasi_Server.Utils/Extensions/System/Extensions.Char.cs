using System.Globalization;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static double GetNumericValue(this char c) => char.GetNumericValue(c);

        public static UnicodeCategory GetUnicodeCategory(this char c) => char.GetUnicodeCategory(c);

        public static char ToLower(this char c) => char.ToLower(c);

        public static char ToLower(this char c, CultureInfo culture) => char.ToLower(c, culture);

        public static char ToLowerInvariant(this char c) => char.ToLowerInvariant(c);

        public static char ToUpper(this char c) => char.ToUpper(c);

        public static char ToUpper(this char c, CultureInfo culture) => char.ToUpper(c, culture);

        public static char ToUpperInvariant(this char c) => char.ToUpperInvariant(c);

        public static int ConvertToUtf32(this char highSurrogate, char lowSurrogate) =>
            char.ConvertToUtf32(highSurrogate, lowSurrogate);

        public static bool IsSurrogate(this char c) => char.IsSurrogate(c);

        public static bool IsSurrogatePair(this char highSurrogate, char lowSurrogate) =>
            char.IsSurrogatePair(highSurrogate, lowSurrogate);

        public static bool IsHighSurrogate(this char c) => char.IsHighSurrogate(c);

        public static bool IsLowSurrogate(this char c) => char.IsLowSurrogate(c);

        public static string Repeat(this char @this, int repeatCount) => new string(@this, repeatCount);
    }
}