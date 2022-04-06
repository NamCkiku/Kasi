using Kasi_Server.Utils.Extensions;
using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.IO
{
    public static class DirectoryHelper
    {
        #region CreateIfNotExists(创建文件夹，如果不存在)

        public static void CreateIfNotExists(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        #endregion CreateIfNotExists(创建文件夹，如果不存在)

        #region IsSubDirectoryOf(是否指定父目录路径的子目录)

        public static bool IsSubDirectoryOf(string parentDirectoryPath, string childDirectoryPath)
        {
            Check.NotNull(parentDirectoryPath, nameof(parentDirectoryPath));
            Check.NotNull(childDirectoryPath, nameof(childDirectoryPath));

            return IsSubDirectoryOf(new DirectoryInfo(parentDirectoryPath), new DirectoryInfo(childDirectoryPath));
        }

        public static bool IsSubDirectoryOf(DirectoryInfo parentDirectory, DirectoryInfo childDirectory)
        {
            Check.NotNull(parentDirectory, nameof(parentDirectory));
            Check.NotNull(childDirectory, nameof(childDirectory));

            if (parentDirectory.FullName == childDirectory.FullName)
            {
                return true;
            }

            var parentOfChild = childDirectory.Parent;
            if (parentOfChild == null)
            {
                return false;
            }

            return IsSubDirectoryOf(parentDirectory, parentOfChild);
        }

        #endregion IsSubDirectoryOf(是否指定父目录路径的子目录)

        #region ChangeCurrentDirectory(更改当前目录)

        public static IDisposable ChangeCurrentDirectory(string targetDirectory)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (currentDirectory.Equals(targetDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return NullDisposable.Instance;
            }

            Directory.SetCurrentDirectory(targetDirectory);

            return new DisposeAction(() => { Directory.SetCurrentDirectory(currentDirectory); });
        }

        #endregion ChangeCurrentDirectory(更改当前目录)

        #region GetFileNames(获取指定目录中的文件列表)

        public static string[] GetFileNames(string directoryPath, string pattern = "*")
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new FileNotFoundException();
            }

            return Directory.GetFiles(directoryPath, pattern);
        }

        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                return Directory.GetFiles(directoryPath, searchPattern,
                    isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        #endregion GetFileNames(获取指定目录中的文件列表)

        #region GetDirectories(获取指定目录中所有子目录列表)

        public static string[] GetDirectories(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new FileNotFoundException();
            }

            return Directory.GetDirectories(directoryPath);
        }

        #endregion GetDirectories(获取指定目录中所有子目录列表)

        #region Contains(查找指定目录中是否存在指定的文件)

        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild = false)
        {
            try
            {
                var fileNames = GetFileNames(directoryPath, searchPattern, isSearchChild);
                return fileNames.Length != 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion Contains(查找指定目录中是否存在指定的文件)

        #region IsEmpty(是否空目录)

        public static bool IsEmpty(string directoryPath)
        {
            try
            {
                var fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                var direcotryNames = GetDirectories(directoryPath);
                return direcotryNames.Length <= 0;
            }
            catch
            {
                return true;
            }
        }

        #endregion IsEmpty(是否空目录)

        #region Copy(递归复制文件夹及文件夹/文件)

        public static void Copy(string sourcePath, string targetPath, string[] searchPatterns = null)
        {
            sourcePath.CheckNotNullOrEmpty(nameof(sourcePath));
            sourcePath.CheckNotNullOrEmpty(nameof(targetPath));

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException("递归复制文件夹时源目录不存在。");
            }

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string[] dirs = Directory.GetDirectories(sourcePath);
            if (dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    Copy(dir, targetPath + dir.Substring(dir.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }

            if (searchPatterns != null && searchPatterns.Length > 0)
            {
                foreach (var searchPattern in searchPatterns)
                {
                    string[] files = Directory.GetFiles(sourcePath, searchPattern);
                    if (files.Length <= 0)
                    {
                        continue;
                    }

                    foreach (var file in files)
                    {
                        File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                    }
                }
            }
            else
            {
                string[] files = Directory.GetFiles(sourcePath);
                if (files.Length <= 0)
                {
                    return;
                }

                foreach (var file in files)
                {
                    File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }
        }

        #endregion Copy(递归复制文件夹及文件夹/文件)

        #region Delete(递归删除目录)

        public static bool Delete(string directory, bool isDeleteRoot = true)
        {
            directory.CheckNotNullOrEmpty(nameof(directory));

            bool flag = false;
            DirectoryInfo dirPathInfo = new DirectoryInfo(directory);
            if (dirPathInfo.Exists)
            {
                foreach (FileInfo fileInfo in dirPathInfo.GetFiles())
                {
                    fileInfo.Delete();
                }

                foreach (DirectoryInfo subDirectory in dirPathInfo.GetDirectories())
                {
                    Delete(subDirectory.FullName);
                }

                if (isDeleteRoot)
                {
                    dirPathInfo.Attributes = FileAttributes.Normal;
                    dirPathInfo.Delete();
                }

                flag = true;
            }

            return flag;
        }

        #endregion Delete(递归删除目录)

        #region SetAttributes(设置目录属性)

        public static void SetAttributes(string directory, FileAttributes attribute, bool isSet)
        {
            directory.CheckNotNullOrEmpty(nameof(directory));

            DirectoryInfo di = new DirectoryInfo(directory);
            if (!di.Exists)
            {
                throw new DirectoryNotFoundException("设置目录属性时指定文件夹不存在");
            }

            if (isSet)
            {
                di.Attributes = di.Attributes | attribute;
            }
            else
            {
                di.Attributes = di.Attributes & ~attribute;
            }
        }

        #endregion SetAttributes(设置目录属性)
    }
}