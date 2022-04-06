using System.Globalization;

namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringTimeSpanHelper
    {
        public static bool Is(
            string str,
            IFormatProvider formatProvider = null,
            Action<TimeSpan> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            if (formatProvider is null)
                formatProvider = DateTimeFormatInfo.CurrentInfo;
            var result = TimeSpan.TryParse(str, formatProvider, out var timeSpan);
            if (result)
                setupAction?.Invoke(timeSpan);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, TimeSpan>> tries,
            IFormatProvider formatProvider = null,
            Action<TimeSpan> setupAction = null) =>
            Helper.IsXXX(str, string.IsNullOrWhiteSpace, (s, act) => Is(s, formatProvider, act), tries,
                setupAction);

        public static TimeSpan To(
            string str,
            TimeSpan defaultVal = default,
            IFormatProvider formatProvider = null)
        {
            if (formatProvider == null)
                formatProvider = DateTimeFormatInfo.CurrentInfo;
            return TimeSpan.TryParse(str, formatProvider, out var timeSpan) ? timeSpan : defaultVal;
        }

        public static TimeSpan To(
            string str,
            IEnumerable<IConversionImpl<string, TimeSpan>> impls,
            IFormatProvider formatProvider = null)
        {
            if (formatProvider is null)
                formatProvider = DateTimeFormatInfo.CurrentInfo;
            return Helper.ToXXX(str, (s, act) => Is(s, formatProvider, act), impls);
        }
    }
}