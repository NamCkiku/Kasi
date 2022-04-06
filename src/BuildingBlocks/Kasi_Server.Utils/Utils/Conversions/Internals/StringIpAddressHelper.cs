using System.Net;

namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringIpAddressHelper
    {
        public static bool Is(
            string str,
            Action<IPAddress> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = IPAddress.TryParse(str, out var address);
            if (result)
                setupAction?.Invoke(address);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, IPAddress>> tries,
            Action<IPAddress> setupAction = null) =>
            Helper.IsXXX(str, string.IsNullOrWhiteSpace, Is, tries, setupAction);

        public static IPAddress To(
            string str,
            IPAddress defaultVal = default) =>
            IPAddress.TryParse(str, out var address) ? address : defaultVal;

        public static IPAddress To(string str, IEnumerable<IConversionImpl<string, IPAddress>> impls) => Helper.ToXXX(str, Is, impls);
    }
}