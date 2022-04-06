using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static string AsString(this MemoryStream ms, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(ms.ToArray());
        }

        public static void FromString(this MemoryStream ms, string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] buffer = encoding.GetBytes(input);
            ms.Write(buffer, 0, buffer.Length);
        }
    }
}