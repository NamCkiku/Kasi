namespace Kasi_Server.Utils.Extensions
{
    public static class DecimalExtensions
    {

        public static decimal Rounding(this decimal value)
        {
            return Math.Round(value, 2);
        }

        public static decimal Rounding(this decimal value, int decimals)
        {
            return Math.Round(value, decimals);
        }

        public static decimal Abs(this decimal value)
        {
            return Math.Abs(value);
        }

        public static IEnumerable<decimal> Abs(this IEnumerable<decimal> values)
        {
            return values.Select(x => x.Abs());
        }

    }
}