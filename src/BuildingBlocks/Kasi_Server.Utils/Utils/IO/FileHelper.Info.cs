using Kasi_Server.Utils.Helpers;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Kasi_Server.Utils.IO
{
    public static partial class FileHelper
    {
        public static string GetExtension(string fileNameWithExtension)
        {
            Check.NotNull(fileNameWithExtension, nameof(fileNameWithExtension));

            var lastDotIndex = fileNameWithExtension.LastIndexOf('.');
            if (lastDotIndex < 0)
                return string.Empty;
            return fileNameWithExtension.Substring(lastDotIndex + 1);
        }

        public static string GetContentType(string ext)
        {
            var dict = Const.FileExtensionDict;
            ext = ext.ToLower();
            if (!ext.StartsWith("."))
            {
                ext = "." + ext;
            }

            dict.TryGetValue(ext, out var contentType);
            return contentType;
        }

        public static string GetVersion(string fileName)
        {
            if (File.Exists(fileName))
            {
                var fvi = FileVersionInfo.GetVersionInfo(fileName);
                return fvi.FileVersion;
            }

            return null;
        }

        public static Encoding GetEncoding(string filePath)
        {
            return GetEncoding(filePath, Encoding.Default);
        }

        public static Encoding GetEncoding(string filePath, Encoding defaultEncoding)
        {
            var targetEncoding = defaultEncoding;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4))
            {
                if (fs != null && fs.Length >= 2)
                {
                    var pos = fs.Position;
                    fs.Position = 0;
                    var buffer = new int[4];
                    buffer[0] = fs.ReadByte();
                    buffer[1] = fs.ReadByte();
                    buffer[2] = fs.ReadByte();
                    buffer[3] = fs.ReadByte();
                    fs.Position = pos;

                    if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                        targetEncoding = Encoding.BigEndianUnicode;
                    }

                    if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        targetEncoding = Encoding.Unicode;
                    }

                    if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                    {
                        targetEncoding = Encoding.UTF8;
                    }
                }
            }

            return targetEncoding;
        }

        public static string GetMd5(string file)
        {
            return HashFile(file, "md5");
        }

        private static string HashFile(string file, string algName)
        {
            if (!File.Exists(file))
            {
                return string.Empty;
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var bytes = HashData(fs, algName);
                return ToHexString(bytes);
            }
        }

        private static byte[] HashData(Stream stream, string algName)
        {
            if (string.IsNullOrWhiteSpace(algName))
            {
                throw new ArgumentNullException(nameof(algName));
            }

            HashAlgorithm algorithm;
            if (string.Compare(algName, "sha1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                algorithm = SHA1.Create();
            }
            else if (string.Compare(algName, "md5", StringComparison.OrdinalIgnoreCase) == 0)
            {
                algorithm = MD5.Create();
            }
            else
            {
                throw new ArgumentException($"{nameof(algName)} 只能使用 sha1 或 md5.");
            }

            var bytes = algorithm.ComputeHash(stream);
            algorithm.Dispose();
            return bytes;
        }

        private static string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string GetSha1(string file)
        {
            return HashFile(file, "sha1");
        }
    }
}