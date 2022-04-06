using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Helpers
{
    public static class Regexs
    {
        public static Dictionary<string, string> GetValues(string input, string pattern, string[] resultPatterns,
            RegexOptions options = RegexOptions.IgnoreCase)
        {
            var result = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(input))
                return result;
            var match = System.Text.RegularExpressions.Regex.Match(input, pattern, options);
            if (match.Success == false)
                return result;
            AddResults(result, match, resultPatterns);
            return result;
        }

        private static void AddResults(Dictionary<string, string> result, Match match, string[] resultPatterns)
        {
            if (resultPatterns == null)
            {
                result.Add(string.Empty, match.Value);
                return;
            }
            foreach (var resultPattern in resultPatterns)
                result.Add(resultPattern, match.Result(resultPattern));
        }

        public static string GetValue(string input, string pattern, string resultPattern = "",
            RegexOptions options = RegexOptions.IgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            var match = System.Text.RegularExpressions.Regex.Match(input, pattern, options);
            if (match.Success == false)
                return string.Empty;
            return string.IsNullOrWhiteSpace(resultPattern) ? match.Value : match.Result(resultPattern);
        }

        public static string[] Split(string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase) =>
            string.IsNullOrWhiteSpace(input)
                ? new string[] { }
                : System.Text.RegularExpressions.Regex.Split(input, pattern, options);

        public static string Replace(string input, string pattern, string replacement,
            RegexOptions options = RegexOptions.IgnoreCase) => string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options);

        public static bool IsMatch(string input, string pattern) => IsMatch(input, pattern, RegexOptions.IgnoreCase);

        public static bool IsMatch(string input, string pattern, RegexOptions options) => Regex.IsMatch(input, pattern, options);
    }
}