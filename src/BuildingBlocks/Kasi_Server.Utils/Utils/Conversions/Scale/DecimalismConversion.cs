namespace Kasi_Server.Utils.Conversions.Scale
{
    public static class DecimalismConversion
    {
        public static string ToBinary(int dec) => Convert.ToString(dec, 2);

        public static string ToHexadecimal(int dec) => Convert.ToString(dec, 16).ToUpper();

        public static string ToHexadecimal(int dec, int formatLength)
        {
            var hex = ToHexadecimal(dec);
            return hex.Length > formatLength ? hex : hex.PadLeft(formatLength, '0');
        }
    }
}