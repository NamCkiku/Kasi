namespace Kasi_Server.Utils.Conversions.Scale
{
    public static class BinaryConversion
    {
        public static int ToDecimalism(string bin) => Convert.ToInt32(bin, 2);

        public static string ToHexadecimal(string bin) => DecimalismConversion.ToHexadecimal(ToDecimalism(bin));
    }
}