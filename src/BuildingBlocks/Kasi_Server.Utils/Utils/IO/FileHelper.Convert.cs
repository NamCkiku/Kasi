using System.Text;

namespace Kasi_Server.Utils.IO
{
    public static partial class FileHelper
    {
        public static string ToString(byte[] data, Encoding encoding = null)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetString(data);
        }

        public static string ToString(Stream stream, Encoding encoding = null, int bufferSize = 1024 * 2,
            bool isCloseStream = true)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (stream.CanRead == false)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var result = reader.ReadToEnd();
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }
        }

        public static async Task<string> ToStringAsync(Stream stream, Encoding encoding = null,
            int bufferSize = 1024 * 2,
            bool isCloseStream = true)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (stream.CanRead == false)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var result = await reader.ReadToEndAsync();
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }
        }

        public static Stream ToStream(string data, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return Stream.Null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return new MemoryStream(ToBytes(data, encoding));
        }

        public static byte[] ToBytes(string data)
        {
            return ToBytes(data, Encoding.UTF8);
        }

        public static byte[] ToBytes(string data, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return new byte[] { };
            }
            return encoding.GetBytes(data);
        }

        public static byte[] ToBytes(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public static async Task<byte[]> ToBytesAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}