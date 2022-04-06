namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static byte Max(this byte val1, byte val2) => Math.Max(val1, val2);

        public static byte Min(this byte val1, byte val2) => Math.Min(val1, val2);
    }
}