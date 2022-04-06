using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class RegexExtensions
    {
        public static string GetGroupValue(this Match match, string group)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));
            if (string.IsNullOrWhiteSpace(group))
                throw new ArgumentNullException(nameof(group));
            var g = match.Groups[group];
            if (!match.Success || !g.Success)
                throw new InvalidOperationException($"未能在匹配结果中找到匹配分组({group})");
            return g.Value;
        }
    }
}