namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringCharHelper
    {
        public static bool Is(
            string str,
            Action<char> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = char.TryParse(str, out var c);
            if (result)
                setupAction?.Invoke(c);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, char>> tries,
            Action<char> setupAction = null) =>
            Helper.IsXXX(str, string.IsNullOrWhiteSpace, Is, tries, setupAction);

        public static char To(
            string str,
            char defaultVal = default) =>
            char.TryParse(str, out var c) ? c : defaultVal;

        public static char To(string str, IEnumerable<IConversionImpl<string, char>> impls) => Helper.ToXXX(str, Is, impls);
    }
}