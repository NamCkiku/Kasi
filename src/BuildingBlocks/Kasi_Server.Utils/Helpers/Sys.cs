using System.Runtime.InteropServices;

namespace Kasi_Server.Utils.Helpers
{
    public static class Sys
    {
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static string System => IsWindows ? "Windows" : IsLinux ? "Linux" : IsOsx ? "OSX" : string.Empty;
    }
}