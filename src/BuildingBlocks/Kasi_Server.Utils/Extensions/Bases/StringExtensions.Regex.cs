using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static string[] RegexSplit(this string value, string pattern, RegexOptions options)
        {
            return Regex.Split(value, pattern, options);
        }

        public static string[] GetWords(this string value)
        {
            return value.RegexSplit(@"\W", RegexOptions.None);
        }

        public static string GetWordByIndex(this string value, int index)
        {
            var words = value.GetWords();
            if (index < 0 || index > words.Length - 1)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            return words[index];
        }

        public static string SpaceOnUpper(this string value)
        {
            return Regex.Replace(value, @"([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)", " $1$2").TrimStart();
        }

        public static string ReplaceWith(this string value, string pattern, string replaceValue)
        {
            return value.ReplaceWith(pattern, replaceValue, RegexOptions.None);
        }

        public static string ReplaceWith(this string value, string pattern, string replaceValue, RegexOptions options)
        {
            return Regex.Replace(value, pattern, replaceValue, options);
        }

        public static string ReplaceWith(this string value, string pattern, MatchEvaluator evaluator)
        {
            return value.ReplaceWith(pattern, RegexOptions.None, evaluator);
        }

        public static string ReplaceWith(this string value, string pattern, RegexOptions options,
            MatchEvaluator evaluator)
        {
            return Regex.Replace(value, pattern, evaluator, options);
        }
    }
}