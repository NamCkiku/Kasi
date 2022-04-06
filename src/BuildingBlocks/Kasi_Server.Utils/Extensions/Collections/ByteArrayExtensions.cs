using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToString(this byte[] value, Encoding encoding)
        {
            encoding = (encoding ?? Encoding.UTF8);
            return encoding.GetString(value);
        }

        public static string ToHexString(this byte[] value)
        {
            var sb = new StringBuilder();
            foreach (var b in value)
            {
                sb.AppendFormat(" {0}", b.ToString("X2").PadLeft(2, '0'));
            }

            return sb.Length > 0 ? sb.ToString().Substring(1) : sb.ToString();
        }

        public static int ToInt(this byte[] value)
        {
            if (value.Length < 4)
            {
                return 0;
            }

            int num = 0;
            if (value.Length >= 4)
            {
                byte[] tempBuffer = new byte[4];
                Buffer.BlockCopy(value, 0, tempBuffer, 0, 4);
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            return num;
        }

        public static long ToLong(this byte[] value)
        {
            if (value.Length < 8)
            {
                return 0;
            }
            long num = 0;
            if (value.Length >= 8)
            {
                byte[] tempBuffer = new byte[8];
                Buffer.BlockCopy(value, 0, tempBuffer, 0, 8);
                num = BitConverter.ToInt64(tempBuffer, 0);
            }
            return num;
        }

        public static string ToBase64String(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static MemoryStream ToMemoryStream(this byte[] value)
        {
            return new MemoryStream(value);
        }

        public static byte[,] Copy(this byte[,] bytes)
        {
            int width = bytes.GetLength(0), height = bytes.GetLength(1);
            byte[,] newBytes = new byte[width, height];
            Array.Copy(bytes, newBytes, bytes.Length);
            return newBytes;
        }
    }
}