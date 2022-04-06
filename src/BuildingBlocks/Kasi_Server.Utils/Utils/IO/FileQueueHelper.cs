using Kasi_Server.Utils.Serializer;

namespace Kasi_Server.Utils.IO
{
    public class FileQueueHelper
    {
        public static void AddFileToEnqueue(string queueDir, string fileName, string fileContent)
        {
            var saveDir = GetSaveDir(queueDir);
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            var savePath = Path.Combine(saveDir, fileName);
            string tempFilePath = $"{savePath}.bak";
            using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(fs);
                }
            }

            File.Copy(tempFilePath, savePath, true);
            File.Delete(tempFilePath);
        }

        private static string GetSaveDir(string queuePath)
        {
            if (!Directory.Exists(queuePath))
            {
                Directory.CreateDirectory(queuePath);
            }

            return Path.Combine(queuePath, DateTime.Now.ToString("yyyyMMddHHmm"));
        }

        public static void RemoveFileFromQueue(string filePath)
        {
            File.Delete(filePath);
        }

        public static void RemoveFileFromQueue(FileInfo fileInfo)
        {
            File.Delete(fileInfo.FullName);
        }

        public static List<FileInfo> GetFilesFromQueue(string queueDir, int takeCount, string type = "")
        {
            var items = new List<FileInfo>();
            if (!Directory.Exists(queueDir))
            {
                return items;
            }

            DirectoryInfo homeDir = new DirectoryInfo(queueDir);
            DirectoryInfo[] dirs = homeDir.GetDirectories().OrderBy(p => Convert.ToInt32(p.Name)).ToArray();
            for (var i = 0; i < dirs.Length; i++)
            {
                DirectoryInfo dir = dirs[i];
                var fileInfos = !string.IsNullOrWhiteSpace(type) ? dir.GetFiles(type) : dir.GetFiles();
                if (fileInfos.Length == 0)
                {
                    if (dir.CreationTime < DateTime.Now.AddMinutes(-2))
                    {
                        var files = dir.GetFiles();
                        if (files.Length == 0)
                        {
                            Directory.Delete(dir.FullName, false);
                        }
                        else
                        {
                            foreach (var file in files)
                            {
                                if (file.Name.EndsWith(".data"))
                                {
                                    continue;
                                }

                                if (file.Name.EndsWith(".bak"))
                                {
                                    file.MoveTo(file.FullName.Replace(".bak", ""));
                                }
                            }
                        }
                    }
                }

                foreach (var fileInfo in fileInfos)
                {
                    items.Add(fileInfo);
                    if (items.Count >= takeCount)
                    {
                        return items;
                    }
                }
            }

            return items;
        }

        public static T ReadObjectFromQueue<T>(string filePath)
        {
            T t = default(T);
            var fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    string content;
                    using (var sr = new StreamReader(fs))
                    {
                        content = sr.ReadToEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        t = JsonHelper.ToObject<T>(content);
                    }

                    fs.Close();
                }
            }

            return t;
        }

        public static string ReadStringFromQueue(string filePath)
        {
            string content = string.Empty;
            var fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        content = sr.ReadToEnd();
                    }
                    fs.Close();
                }
            }

            return content;
        }

        public static DirectoryInfo[] GetQueueDirs(string queueDir)
        {
            DirectoryInfo homeDir = new DirectoryInfo(queueDir);
            return homeDir.GetDirectories();
        }
    }
}