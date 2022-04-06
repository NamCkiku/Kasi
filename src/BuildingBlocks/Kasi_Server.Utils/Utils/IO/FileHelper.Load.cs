using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.IO
{
    public static partial class FileHelper
    {
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            Check.NotNull(filePath, nameof(filePath));
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            Check.NotNull(filePath, nameof(filePath));
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
                return result;
            }
        }
    }
}