using System.Text;

namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class EncodingHelper
    {
        public static Encoding Fixed(this Encoding encoding) => encoding ?? Encoding.UTF8;
    }
}