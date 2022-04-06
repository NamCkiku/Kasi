using System.Text;

namespace Kasi_Server.Utils.Conversions.Scale
{
    public static class BytesConversion
    {
        public static string ToAscii(byte[] bytes) => Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        public static string ToBinary(byte @byte) => DecimalismConversion.ToBinary(@byte);

        public static int ToDecimalism(byte h, byte l) => h << 8 | l;

        public static int ToDecimalism(byte h, byte l, bool isRadix)
        {
            var v = (ushort)(h << 0 | l);
            if (isRadix && h > 127)
            {
                v = (ushort)~v;
                v = (ushort)(v + 1);
                return -1 * v;
            }
            return v;
        }

        public static string ToHexadecimal(byte @byte) => @byte.ToString("X2");

        public static string ToHexadecimal(byte[] bytes)
        {
            var ret = "";
            if (bytes != null)
                ret = bytes.Aggregate(ret, (current, t) => $"{current}{t:X2}");
            return ret;
        }

        public static string ToHexadecimal(byte h, byte l) => $"{ToHexadecimal(h)}{ToHexadecimal(l)}";
    }
}