namespace Kasi_Server.Utils.Extensions
{
    public static partial class BaseTypeExtensions
    {
        public static bool In(this char @this, params char[] values) => Array.IndexOf(values, @this) != -1;

        public static bool NotIn(this char @this, params char[] values) => Array.IndexOf(values, @this) == -1;

        public static bool IsWhiteSpace(this char c) => char.IsWhiteSpace(c);

        public static bool IsControl(this char c) => char.IsControl(c);

        public static bool IsDigit(this char c) => char.IsDigit(c);

        public static bool IsLetter(this char c) => char.IsLetter(c);

        public static bool IsLetterOrDigit(this char c) => char.IsLetterOrDigit(c);

        public static bool IsLower(this char c) => char.IsLower(c);

        public static bool IsNumber(this char c) => char.IsNumber(c);

        public static bool IsPunctuation(this char c) => char.IsPunctuation(c);

        public static bool IsSeparator(this char c) => char.IsSeparator(c);

        public static bool IsSymbol(this char c) => char.IsSymbol(c);
    }
}