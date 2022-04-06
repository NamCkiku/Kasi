using System.Security.Cryptography;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class StreamExtensions
    {
        public static bool ToFile(this Stream stream, string path)
        {
            if (stream == null)
            {
                return false;
            }

            const int bufferSize = 32768;
            bool result = true;
            Stream fileStream = null;
            byte[] buffer = new byte[bufferSize];
            try
            {
                using (fileStream = File.OpenWrite(path))
                {
                    int len;
                    while ((len = stream.Read(buffer, 0, bufferSize)) > 0)
                    {
                        fileStream.Write(buffer, 0, len);
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            return (result && File.Exists(path));
        }

        public static bool ContentsEqual(this Stream stream, Stream other)
        {
            stream.CheckNotNull(nameof(stream));
            other.CheckNotNull(nameof(other));

            if (stream.Length != other.Length)
            {
                return false;
            }

            const int bufferSize = 2048;
            byte[] streamBuffer = new byte[bufferSize];
            byte[] otherBuffer = new byte[bufferSize];

            while (true)
            {
                int streamLen = stream.Read(streamBuffer, 0, bufferSize);
                int otherLen = other.Read(otherBuffer, 0, bufferSize);

                if (streamLen != otherLen)
                {
                    return false;
                }

                if (streamLen == 0)
                {
                    return true;
                }

                int iterations = (int)Math.Ceiling((double)streamLen / sizeof(Int64));
                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(streamBuffer, i * sizeof(Int64)) !=
                        BitConverter.ToInt64(otherBuffer, i * sizeof(Int64)))
                    {
                        return false;
                    }
                }
            }
        }

        public static StreamReader GetReader(this Stream stream)
        {
            return GetReader(stream, null);
        }

        public static StreamReader GetReader(this Stream stream, Encoding encoding)
        {
            if (stream.CanRead == false)
            {
                throw new InvalidOperationException("Stream 不支持读取操作");
            }
            encoding = encoding ?? Encoding.UTF8;
            return new StreamReader(stream, encoding);
        }

        public static StreamWriter GetWriter(this Stream stream)
        {
            return GetWriter(stream, null);
        }

        public static StreamWriter GetWriter(this Stream stream, Encoding encoding)
        {
            if (stream.CanWrite == false)
            {
                throw new InvalidOperationException("Stream 不支持写入操作");
            }

            encoding = encoding ?? Encoding.UTF8;
            return new StreamWriter(stream, encoding);
        }

        public static string ReadToEnd(this Stream stream)
        {
            return ReadToEnd(stream, null);
        }

        public static string ReadToEnd(this Stream stream, Encoding encoding)
        {
            using (var reader = stream.GetReader(encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream SeekToBegin(this Stream stream)
        {
            if (stream.CanSeek == false)
            {
                throw new InvalidOperationException("Stream 不支持寻址操作");
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream SeekToEnd(this Stream stream)
        {
            if (stream.CanSeek == false)
            {
                throw new InvalidOperationException("Stream 不支持寻址操作");
            }

            stream.Seek(0, SeekOrigin.End);
            return stream;
        }

        public static MemoryStream CopyToMemory(this Stream stream)
        {
            var memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);
            return memoryStream;
        }

        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var memoryStream = stream.CopyToMemory())
            {
                return memoryStream.ToArray();
            }
        }

        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void Write(this Stream stream, string context, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(context);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static string GetMd5(this Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var buffer = md5.ComputeHash(stream);
                var md5Builder = new StringBuilder();
                foreach (var b in buffer)
                {
                    md5Builder.Append(b.ToString("x2"));
                }

                return md5Builder.ToString();
            }
        }
    }
}