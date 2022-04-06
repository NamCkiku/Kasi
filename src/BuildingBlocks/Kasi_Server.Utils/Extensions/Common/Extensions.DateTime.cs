using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            return dateTime.ToString(isRemoveSecond ? "yyyy-MM-dd HH:mm" : "yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            return dateTime == null ? string.Empty : ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToDateString(dateTime.Value);
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        public static string ToTimeString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToTimeString(dateTime.Value);
        }

        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string ToMillisecondString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToMillisecondString(dateTime.Value);
        }

        public static string ToChineseDateString(this DateTime dateTime)
        {
            return $"{dateTime.Year}年{dateTime.Month}月{dateTime.Day}日";
        }

        public static string ToChineseDateString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToChineseDateString(dateTime.Value);
        }

        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            var result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
            {
                result.AppendFormat("{0}秒", dateTime.Second);
            }

            return result.ToString();
        }

        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            return dateTime == null ? string.Empty : ToChineseDateTimeString(dateTime.Value, isRemoveSecond);
        }

        public static TimeSpan DateDiff(this DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            return ts1.Subtract(ts2).Duration();
        }

        public static string Description(this TimeSpan span)
        {
            var result = new StringBuilder();
            if (span.Days > 0)
            {
                result.AppendFormat("{0}天", span.Days);
            }

            if (span.Hours > 0)
            {
                result.AppendFormat("{0}小时", span.Hours);
            }

            if (span.Minutes > 0)
            {
                result.AppendFormat("{0}分", span.Minutes);
            }

            if (span.Seconds > 0)
            {
                result.AppendFormat("{0}秒", span.Seconds);
            }

            if (span.Milliseconds > 0)
            {
                result.AppendFormat("{0}毫秒", span.Milliseconds);
            }

            if (result.Length > 0)
            {
                return result.ToString();
            }

            return $"{span.TotalSeconds * 1000}毫秒";
        }
    }
}