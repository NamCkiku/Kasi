namespace Kasi_Server.Utils.Helpers
{
    public static class UnixTime
    {
        public static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToTimestamp(bool isContainMillisecond = true)
        {
            return ToTimestamp(DateTime.Now, isContainMillisecond);
        }

        public static long ToTimestamp(DateTime dateTime, bool isContainMillisecond = true)
        {
            if (isContainMillisecond)
                return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            else
                return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        public static DateTime ToDateTime(long unixTimeStamp, DateTimeKind dateTimeKind = DateTimeKind.Local)
        {
            if (unixTimeStamp.ToString().Length == 10)
                return dateTimeKind == DateTimeKind.Local ?
                    DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).LocalDateTime :
                    DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
            else
                return dateTimeKind == DateTimeKind.Local ?
                    DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).LocalDateTime :
                    DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).UtcDateTime;
        }
    }
}