namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static void BlockCopy(this Array src, int srcOffset, Array dst, int dstOffset, int count) => Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);

        public static int ByteLength(this Array array) => Buffer.ByteLength(array);

        public static byte GetByte(this Array array, int index) => Buffer.GetByte(array, index);

        public static void SetByte(this Array array, int index, byte value) => Buffer.SetByte(array, index, value);
    }
}