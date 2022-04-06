namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static byte[] Resize(this byte[] @this, int newSize)
        {
            Array.Resize(ref @this, newSize);
            return @this;
        }

        public static int ToBase64CharArray(this byte[] inArray, int offsetIn, int length, char[] outArray,
            int offsetOut) => Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut);

        public static int ToBase64CharArray(this byte[] inArray, int offsetIn, int length, char[] outArray,
            int offsetOut, Base64FormattingOptions options) =>
            Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, options);

        public static string ToBase64String(this byte[] inArray, Base64FormattingOptions options) => Convert.ToBase64String(inArray, options);

        public static string ToBase64String(this byte[] inArray, int offset, int length) => Convert.ToBase64String(inArray, offset, length);

        public static string ToBase64String(this byte[] inArray, int offset, int length,
            Base64FormattingOptions options) => Convert.ToBase64String(inArray, offset, length, options);

        public static MemoryStream ToMemoryStream(this byte[] @this) => new MemoryStream(@this);
    }
}