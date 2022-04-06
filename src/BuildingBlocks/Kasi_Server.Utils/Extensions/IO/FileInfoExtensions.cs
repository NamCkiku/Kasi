namespace Kasi_Server.Utils.Extensions
{
    public static class FileInfoExtensions
    {
        public static bool CompareTo(this FileInfo file1, FileInfo file2)
        {
            if (file1 == null || !file1.Exists)
            {
                throw new ArgumentNullException(nameof(file1));
            }

            if (file2 == null || !file2.Exists)
            {
                throw new ArgumentNullException(nameof(file2));
            }

            if (file1.Length != file2.Length)
            {
                return false;
            }

            return file1.Read().Equals(file2.Read());
        }

        public static string Read(this FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Exists == false)
            {
                return string.Empty;
            }

            using (var reader = file.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        public static byte[] ReadBinary(this FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Exists == false)
            {
                return new byte[0];
            }

            using (var reader = file.OpenRead())
            {
                return reader.ReadAllBytes();
            }
        }
    }
}