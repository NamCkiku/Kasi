using Kasi_Server.Utils.Extensions;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using FileInfo = System.IO.FileInfo;

namespace Kasi_Server.Utils.IO
{
    public static partial class FileHelper
    {
        public static void Create(string filePath, string content)
        {
            var tmpPath = $"{filePath}.tmp";

            FileInfo fi = new FileInfo(tmpPath);

            if (!fi.Directory.Exists)
                fi.Directory.Create();

            using (var fs = fi.OpenWrite())
                fs.Write(content, Encoding.UTF8);

            fi.CopyTo(filePath, true);

            fi.Delete();
        }

        public static void CreateIfNotExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                return;
            }
            File.Create(fileName);
        }

        public static void Delete(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                Delete(filePath);
            }
        }

        public static void Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
        }

        public static bool KillFile(string fileName, int deleteCount, bool randomData = true, bool blanks = false)
        {
            const int bufferLength = 1024000;
            bool ret = true;
            try
            {
                using (
                    FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                {
                    FileInfo file = new FileInfo(fileName);
                    long count = file.Length;
                    long offset = 0;
                    var rowDataBuffer = new byte[bufferLength];
                    while (count >= 0)
                    {
                        int iNumOfDataRead = stream.Read(rowDataBuffer, 0, bufferLength);
                        if (iNumOfDataRead == 0)
                        {
                            break;
                        }

                        if (randomData)
                        {
                            Random randomByte = new Random();
                            randomByte.NextBytes(rowDataBuffer);
                        }
                        else if (blanks)
                        {
                            for (int i = 0; i < iNumOfDataRead; i++)
                            {
                                rowDataBuffer[i] = Convert.ToByte(Convert.ToChar(deleteCount));
                            }
                        }

                        for (int i = 0; i < deleteCount; i++)
                        {
                            stream.Seek(offset, SeekOrigin.Begin);
                            stream.Write(rowDataBuffer, 0, iNumOfDataRead);
                            ;
                        }

                        offset += iNumOfDataRead;
                        count -= iNumOfDataRead;
                    }
                }

                string newName = "";
                do
                {
                    Random random = new Random();
                    string cleanName = Path.GetFileName(fileName);
                    string dirName = Path.GetDirectoryName(fileName);
                    int iMoreRandomLetters = random.Next(9);
                    for (int i = 0; i < cleanName.Length + iMoreRandomLetters; i++)
                    {
                        newName += random.Next(9).ToString();
                    }

                    newName = dirName + "\\" + newName;
                } while (File.Exists(newName));

                File.Move(fileName, newName);
                File.Delete(newName);
            }
            catch
            {
                try
                {
                    string filename = fileName;
                    Process tool = new Process()
                    {
                        StartInfo =
                        {
                            FileName = "handle.exe",
                            Arguments = filename + " /accepteula",
                            UseShellExecute = false,
                            RedirectStandardOutput = true
                        }
                    };
                    tool.Start();
                    tool.WaitForExit();
                    string outputTool = tool.StandardOutput.ReadToEnd();
                    string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
                    foreach (Match match in Regex.Matches(outputTool, matchPattern))
                    {
                        Process.GetProcessById(int.Parse(match.Value)).Kill();
                    }

                    File.Delete(filename);
                }
                catch
                {
                    ret = false;
                }
            }

            return ret;
        }

        public static void SetAttribute(string fileName, FileAttributes attribute, bool isSet)
        {
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new FileNotFoundException("要设置属性的文件不存在。", fileName);
            }

            if (isSet)
            {
                fi.Attributes = fi.Attributes | attribute;
            }
            else
            {
                fi.Attributes = fi.Attributes & ~attribute;
            }
        }

        public static List<string> GetAllFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).ToList();
        }

        public static string Read(string filePath)
        {
            return Read(filePath, Encoding.UTF8);
        }

        public static string Read(string filePath, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (!File.Exists(filePath))
                return string.Empty;
            using (var reader = new StreamReader(filePath, encoding))
                return reader.ReadToEnd();
        }

        public static byte[] ReadToBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return ReadToBytes(new FileInfo(filePath));
        }

        public static byte[] ReadToBytes(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            int fileSize = (int)fileInfo.Length;
            using (var reader = new BinaryReader(fileInfo.Open(FileMode.Open)))
            {
                return reader.ReadBytes(fileSize);
            }
        }

        public static void Write(string filePath, string content)
        {
            Write(filePath, ToBytes(content.SafeString()));
        }

        public static void Write(string filePath, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;
            if (bytes == null)
                return;
            File.WriteAllBytes(filePath, bytes);
        }

        public static string JoinPath(string basePath, string subPath)
        {
            basePath = basePath.TrimEnd('/').TrimEnd('\\');
            subPath = subPath.TrimStart('/').TrimStart('\\');
            string path = basePath + "\\" + subPath;
            return path.Replace("/", "\\").ToLower();
        }

        public static async Task<string> CopyToStringAsync(Stream stream, Encoding encoding = null)
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

            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream, encoding))
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    stream.CopyTo(memoryStream);
                    if (memoryStream.CanSeek)
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }

                    var result = await reader.ReadToEndAsync();
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    return result;
                }
            }
        }

        public static void Combine(IList<string> files, string fileName, bool delete = false, bool encrypt = false, int sign = 0)
        {
            if (files == null || files.Count == 0)
            {
                return;
            }

            files.Sort();
            using (var ws = new FileStream(fileName, FileMode.Create))
            {
                foreach (var file in files)
                {
                    if (file == null || !File.Exists(file))
                    {
                        continue;
                    }

                    using (var rs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        var data = new byte[1024];
                        var readLen = 0;
                        while ((readLen = rs.Read(data, 0, data.Length)) > 0)
                        {
                            ws.Write(data, 0, readLen);
                            ws.Flush();
                        }
                    }
                    if (delete)
                    {
                        Delete(file);
                    }
                }
            }
        }

        private static int GetSplitFileTotal(int fileSize, int splitSize)
        {
            fileSize = fileSize / 1024;
            if (fileSize % splitSize == 0)
            {
                return fileSize / splitSize;
            }

            return fileSize / splitSize + 1;
        }

        public static bool Compress(string file, string saveFile)
        {
            if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(saveFile))
            {
                return false;
            }

            if (!File.Exists(file))
            {
                return false;
            }

            try
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    using (var ws = new FileStream(saveFile, FileMode.Create))
                    {
                        using (var zip = new GZipStream(ws, CompressionMode.Compress))
                        {
                            fs.CopyTo(zip);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Decompress(string file, string saveFile)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(saveFile))
            {
                return false;
            }

            if (!File.Exists(file))
            {
                return false;
            }

            try
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    using (var ws = new FileStream(saveFile, FileMode.Create))
                    {
                        using (var zip = new GZipStream(fs, CompressionMode.Decompress))
                        {
                            zip.CopyTo(ws);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void CompressMulti(string[] sourceFileList, string saveFullPath)
        {
            if (sourceFileList == null || sourceFileList.Length == 0 || string.IsNullOrWhiteSpace(saveFullPath))
            {
                return;
            }

            using (var ms = new MemoryStream())
            {
                foreach (var filePath in sourceFileList)
                {
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }

                    string fileName = Path.GetFileName(filePath);
                    byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
                    byte[] sizeBytes = BitConverter.GetBytes(fileNameBytes.Length);
                    ms.Write(sizeBytes, 0, sizeBytes.Length);
                    ms.Write(fileNameBytes, 0, fileNameBytes.Length);
                    byte[] fileContentBytes = File.ReadAllBytes(filePath);
                    ms.Write(BitConverter.GetBytes(fileContentBytes.Length), 0, 4);
                    ms.Write(fileContentBytes, 0, fileContentBytes.Length);
                }

                ms.Flush();
                ms.Position = 0;

                using (var fs = File.Create(saveFullPath))
                {
                    using (var zipStream = new GZipStream(fs, CompressionMode.Compress))
                    {
                        ms.Position = 0;
                        ms.CopyTo(zipStream);
                    }
                }
            }
        }

        public static void DecompressMulti(string zipPath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(zipPath) || string.IsNullOrWhiteSpace(targetPath))
            {
                return;
            }

            byte[] fileSize = new byte[4];
            if (!File.Exists(zipPath))
            {
                return;
            }

            using (var fs = File.Open(zipPath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    using (var zipStream = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(ms);
                    }

                    ms.Position = 0;
                    while (ms.Position != ms.Length)
                    {
                        ms.Read(fileSize, 0, fileSize.Length);
                        var fileNameLength = BitConverter.ToInt32(fileSize, 0);
                        var fileNameBytes = new byte[fileNameLength];
                        ms.Read(fileNameBytes, 0, fileNameBytes.Length);
                        var fileName = Encoding.UTF8.GetString(fileNameBytes);
                        var fileFullName = targetPath + fileName;
                        ms.Read(fileSize, 0, 4);
                        var fileContentLength = BitConverter.ToInt32(fileSize, 0);
                        var fileContentBytes = new byte[fileContentLength];
                        ms.Read(fileContentBytes, 0, fileContentBytes.Length);
                        using (var childFileStream = File.Create(fileFullName))
                        {
                            childFileStream.Write(fileContentBytes, 0, fileContentBytes.Length);
                        }
                    }
                }
            }
        }
    }

    public enum WriteType
    {
        Append = 1,

        Covered = 2
    }
}