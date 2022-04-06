namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static DateTime SetDateTime(this DateTime date, int hours, int minutes, int seconds) =>
            date.SetDateTime(new TimeSpan(hours, minutes, seconds));

        public static DateTime SetDateTime(this DateTime date, TimeSpan time) => date.Date.Add(time);

        public static DateTimeOffset SetDateTime(this DateTimeOffset date, int hours, int minutes, int seconds) =>
            date.SetDateTime(new TimeSpan(hours, minutes, seconds));

        public static DateTimeOffset SetDateTime(this DateTimeOffset date, TimeSpan time) => date.SetDateTime(time, null);

        public static DateTimeOffset SetDateTime(this DateTimeOffset date, TimeSpan time, TimeZoneInfo localTimeZone) =>
            date.ToLocalDateTime(localTimeZone).SetDateTime(time).ToDateTimeOffset(localTimeZone);

        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime) =>
            localDateTime.ToDateTimeOffset(null);

        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone)
        {
            if (localDateTime.Kind != DateTimeKind.Unspecified)
                localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(localDateTime, localTimeZone ?? TimeZoneInfo.Local);
        }

        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc) => dateTimeUtc.ToLocalDateTime(null);

        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone) =>
            TimeZoneInfo.ConvertTime(dateTimeUtc, localTimeZone ?? TimeZoneInfo.Local).DateTime;
    }
}