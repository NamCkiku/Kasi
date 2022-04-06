namespace Kasi_Server.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static bool InRange(this float value, float minValue, float maxValue)
        {
            return (value >= minValue && value <= maxValue);
        }

        public static float InRange(this float value, float minValue, float maxValue, float defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }

        public static TimeSpan Days(this float days)
        {
            return TimeSpan.FromDays(days);
        }

        public static TimeSpan Hours(this float hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Minutes(this float minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public static TimeSpan Seconds(this float seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan Milliseconds(this float milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }
    }
}