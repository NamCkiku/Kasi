namespace Kasi_Server.Utils.Extensions
{
    public static class IntExtensions
    {
        public static void Times(this int value, Action action)
        {
            value.AsLong().Times(action);
        }

        public static void Times(this int value, Action<int> action)
        {
            for (var i = 0; i < value; i++)
            {
                action(i);
            }
        }

        public static bool IsEven(this int value)
        {
            return value.AsLong().IsEven();
        }

        public static bool IsOdd(this int value)
        {
            return value.AsLong().IsOdd();
        }

        public static bool InRange(this int value, int minValue, int maxValue)
        {
            return value.AsLong().InRange(minValue, maxValue);
        }

        public static int InRange(this int value, int minValue, int maxValue, int defaultValue)
        {
            return (int)value.AsLong().InRange(minValue, maxValue, defaultValue);
        }

        public static bool IsPrime(this int value)
        {
            return value.AsLong().IsPrime();
        }

        public static string ToOrdinal(this int i)
        {
            return i.AsLong().ToOrdinal();
        }

        public static string ToOrdinal(this int i, string format)
        {
            return i.AsLong().ToOrdinal(format);
        }

        public static long AsLong(this int i)
        {
            return i;
        }

        public static bool IsIndexInArray(this int index, Array array)
        {
            return index.GetArrayIndex().InRange(array.GetLowerBound(0), array.GetUpperBound(0));
        }

        public static int GetArrayIndex(this int at)
        {
            return at == 0 ? 0 : at - 1;
        }

        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static TimeSpan Ticks(this int ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }
    }
}