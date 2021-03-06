using System.Globalization;

namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringShortHelper
    {
        public static bool Is(
            string str,
            NumberStyles style = NumberStyles.Integer,
            IFormatProvider formatProvider = null,
            Action<short> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            if (formatProvider is null)
                formatProvider = NumberFormatInfo.CurrentInfo;
            var result = short.TryParse(str, style, formatProvider, out var number);
            if (result)
                setupAction?.Invoke(number);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, short>> tries,
            NumberStyles style = NumberStyles.Integer,
            IFormatProvider formatProvider = null,
            Action<short> setupAction = null)
        {
            if (formatProvider is null)
                formatProvider = NumberFormatInfo.CurrentInfo;
            return Helper.IsXXX(str, string.IsNullOrWhiteSpace, (s, act) => Is(s, style, formatProvider, act), tries,
                setupAction);
        }

        public static short To(
            string str,
            short defaultVal = default,
            NumberStyles style = NumberStyles.Integer,
            IFormatProvider formatProvider = null)
        {
            if (formatProvider == null)
                formatProvider = NumberFormatInfo.CurrentInfo;
            return short.TryParse(str, style, formatProvider, out var number) ? number : defaultVal;
        }

        public static short To(
            string str,
            IEnumerable<IConversionImpl<string, short>> impls,
            NumberStyles style = NumberStyles.Integer,
            IFormatProvider formatProvider = null)
        {
            if (formatProvider is null)
                formatProvider = NumberFormatInfo.CurrentInfo;
            return Helper.ToXXX(str, (s, act) => Is(s, style, formatProvider, act), impls);
        }
    }
}