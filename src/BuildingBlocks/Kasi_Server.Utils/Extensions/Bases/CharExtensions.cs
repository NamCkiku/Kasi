using System.Text;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Extensions
{
    public static class CharExtensions
    {
        public static bool In(this char @this, params char[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool NotIn(this char @this, params char[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        public static string Repeat(this char @this, int repeatCount)
        {
            return new string(@this, repeatCount);
        }

        public static int GetAsciiCode(this char value)
        {
            byte[] bytes = Encoding.GetEncoding(0).GetBytes(value.ToString());
            if (bytes.Length == 1)
            {
                return bytes[0];
            }

            return (((bytes[0] * 0x100) + bytes[1]) - 0x10000);
        }

        public static bool IsChinese(this char value)
        {
            return Regex.IsMatch(value.ToString(), "^[一-龥]$");
        }

        public static bool IsLine(this char value)
        {
            if (value != '\r')
            {
                return (value == '\n');
            }

            return true;
        }

        public static bool IsDoubleByte(this char value)
        {
            return Regex.IsMatch(value.ToString(), @"[^\x00-\xff]");
        }

        public static char ToDBC(this char value)
        {
            if (value == 12288)
            {
                value = (char)32;
            }

            if (value > 65280 && value < 65375)
            {
                value = (char)(value - 65248);
            }

            return value;
        }

        public static char ToSBC(this char value)
        {
            if (value == 32)
            {
                value = (char)12288;
            }

            if (value < 127)
            {
                value = (char)(value + 65248);
            }

            return value;
        }

    }
}