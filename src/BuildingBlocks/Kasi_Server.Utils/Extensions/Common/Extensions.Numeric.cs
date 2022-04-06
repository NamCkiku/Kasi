namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static float KeepDigits(this float value, int digits) =>
            (float)Math.Round((decimal)value, digits, MidpointRounding.AwayFromZero);

        public static double KeepDigits(this double value, int digits) =>
            (double)Math.Round((decimal)value, digits, MidpointRounding.AwayFromZero);

        public static decimal KeepDigits(this decimal value, int digits) =>
            Math.Round(value, digits, MidpointRounding.AwayFromZero);

        public static bool IsIn(this byte value, byte min, byte max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this short value, short min, short max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this int value, int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this long value, long min, long max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this double value, double min, double max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }

        public static bool IsIn(this decimal value, decimal min, decimal max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), @"最小值不可大于最大值！");
            return value >= min && value <= max;
        }
    }
}