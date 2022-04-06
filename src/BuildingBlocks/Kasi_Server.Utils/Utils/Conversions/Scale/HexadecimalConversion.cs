using Kasi_Server.Utils.Conversions.Internals;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Conversions.Scale
{
    public static class HexadecimalConversion
    {
        public static int ToDecimalism(string hex) => Convert.ToInt32(hex, 16);

        public static string ToBinary(string hex) => DecimalismConversion.ToBinary(ToDecimalism(hex));

        public static byte[] ToBytes(string hex)
        {
            var mc = Regex.Matches(hex, @"(?i)[\da-f]{2}");
            return (from Match m in mc select Convert.ToByte(m.Value, 16)).ToArray();
        }

        public static string ToString(string hex, Encoding encoding = null)
        {
            hex = hex.Replace(" ", "");
            if (string.IsNullOrWhiteSpace(hex))
                return "";
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
            {
                if (!byte.TryParse(hex.Substring(i, 2), NumberStyles.HexNumber, null, out bytes[i / 2]))
                    bytes[i / 2] = 0;
            }
            return encoding.Fixed().GetString(bytes);
        }

        public static string FromString(string str, Encoding encoding = null) => BitConverter.ToString(encoding.Fixed().GetBytes(str)).Replace("-", " ");
    }
}