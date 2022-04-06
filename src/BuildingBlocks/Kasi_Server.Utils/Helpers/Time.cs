namespace Kasi_Server.Utils.Helpers
{
    public static class Time
    {
        private static DateTime? _dateTime;

        public static void SetTime(DateTime? dateTime) => _dateTime = dateTime;

        public static void SetTime(string dateTime) => _dateTime = Conv.ToDateOrNull(dateTime);

        public static void Reset() => _dateTime = null;

        public static DateTime GetDateTime() => _dateTime ?? DateTime.Now;

        public static DateTime GetDate() => GetDateTime().Date;

        public static long GetUnixTimestamp() => GetUnixTimestamp(DateTime.Now);

        public static long GetUnixTimestamp(DateTime time) => UnixTime.ToTimestamp(time);

        public static DateTime GetTimeFromUnixTimestamp(long timestamp) => UnixTime.ToDateTime(timestamp, DateTimeKind.Local);
    }
}