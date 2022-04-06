namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class StringBooleanHelper
    {
        public static bool Is(string str, Action<bool> setupAction = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = bool.TryParse(str, out var boolean);
            if (result)
                setupAction?.Invoke(boolean);
            return result;
        }

        public static bool Is(
            string str,
            IEnumerable<IConversionTry<string, bool>> tries,
            Action<bool> setupAction = null) =>
            Helper.IsXXX(str, string.IsNullOrWhiteSpace, Is, tries, setupAction);

        public static bool To(string str, bool defaultVal = default)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultVal;
            return bool.TryParse(str, out var boolean) ? boolean : defaultVal;
        }

        public static bool To(string str, IEnumerable<IConversionImpl<string, bool>> impls) =>
            Helper.ToXXX(str, Is, impls);
    }
}