namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringVersionHelper
    {
        public static bool Is(
            string str,
            Action<Version> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = Version.TryParse(str, out var c);
            if (result)
                setupAction?.Invoke(c);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, Version>> tries,
            Action<Version> setupAction = null) =>
            Helper.IsXXX(str, string.IsNullOrWhiteSpace, Is, tries, setupAction);

        public static Version To(
            string str,
            Version defaultVal = default) =>
            Version.TryParse(str, out var c) ? c : defaultVal;

        public static Version To(string str, IEnumerable<IConversionImpl<string, Version>> impls) => Helper.ToXXX(str, Is, impls);
    }
}