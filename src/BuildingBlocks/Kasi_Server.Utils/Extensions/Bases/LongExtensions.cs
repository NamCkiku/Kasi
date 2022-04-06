using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Extensions
{
    public static class LongExtensions
    {
        public static void Times(this long value, Action action)
        {
            for (var i = 0; i < value; i++)
            {
                action();
            }
        }

        public static void Times(this long value, Action<long> action)
        {
            for (var i = 0; i < value; i++)
            {
                action(i);
            }
        }

        public static bool IsEven(this long value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(this long value)
        {
            return value % 2 != 0;
        }

        public static bool InRange(this long value, long minValue, long maxValue)
        {
            return (value >= minValue && value <= maxValue);
        }

        public static long InRange(this long value, long minValue, long maxValue, long defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }

        public static bool IsPrime(this long value)
        {
            if ((value & 1) == 0)
            {
                if (value == 2)
                {
                    return true;
                }
                return false;
            }
            for (long i = 3; (i * i) <= value; i += 2)
            {
                if ((value % i) == 0)
                {
                    return false;
                }
            }
            return value != 1;
        }

        public static string ToOrdinal(this long i)
        {
            string suffix = "th";
            switch (i % 100)
            {
                case 11:
                case 12:
                case 13:
                    break;

                default:
                    switch (i % 10)
                    {
                        case 1:
                            suffix = "st";
                            break;

                        case 2:
                            suffix = "nd";
                            break;

                        case 3:
                            suffix = "rd";
                            break;
                    }
                    break;
            }
            return $"{i}{suffix}";
        }

        public static string ToOrdinal(this long i, string format)
        {
            return string.Format(format, i.ToOrdinal());
        }

        public static TimeSpan Days(this long days)
        {
            return TimeSpan.FromDays(days);
        }

        public static TimeSpan Hours(this long hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Minutes(this long minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public static TimeSpan Seconds(this long seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan Milliseconds(this long milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static TimeSpan Ticks(this long ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        public static DateTime ToDateTime(this long unixTimeStamp, DateTimeKind dateTimeKind = DateTimeKind.Local) => UnixTime.ToDateTime(unixTimeStamp, dateTimeKind);
    }
}