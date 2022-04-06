using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static void Required<T>(this T value, Func<T, bool> assertionFunc, string message) => Check.Required<T>(value, assertionFunc, message);

        public static void Required<T, TException>(this T value, Func<T, bool> assertionFunc, string message)
            where TException : Exception =>
            Check.Required<T, TException>(value, assertionFunc, message);

        public static void CheckNotNull<T>(this T value, string paramName) where T : class
        {
            Check.NotNull<T>(value, paramName);
        }

        public static void CheckNotNullOrEmpty(this string value, string paramName)
        {
            Check.NotNullOrEmpty(value, paramName);
        }

        public static void CheckNotEmpty(this Guid value, string paramName)
        {
            Check.NotEmpty(value, paramName);
        }

        public static void CheckNotNullOrEmpty<T>(this IEnumerable<T> collection, string paramName)
        {
            Check.NotNullOrEmpty<T>(collection, paramName);
        }

        public static void CheckLessThan<T>(this T value, string paramName, T target, bool canEqual = false)
            where T : IComparable<T>
        {
            Check.LessThan<T>(value, paramName, target, canEqual);
        }

        public static void CheckGreaterThan<T>(this T value, string paramName, T target, bool canEqual = false)
            where T : IComparable<T>
        {
            Check.GreaterThan<T>(value, paramName, target, canEqual);
        }

        public static void CheckBetween<T>(this T value, string paramName, T start, T end, bool startEqual = false,
            bool endEqual = false) where T : IComparable<T>
        {
            Check.Between<T>(value, paramName, start, end, startEqual, endEqual);
        }

        public static void CheckDirectoryExists(this string directory, string paramName = null)
        {
            Check.DirectoryExists(directory, paramName);
        }

        public static void CheckFileExists(this string fileName, string paramName = null)
        {
            Check.FileExists(fileName, paramName);
        }
    }
}