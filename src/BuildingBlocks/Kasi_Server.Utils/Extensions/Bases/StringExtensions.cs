using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Enum = System.Enum;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static string Repeat(this string value, int repeatCount)
        {
            if (string.IsNullOrEmpty(value) || repeatCount == 0)
                return string.Empty;
            if (value.Length == 1)
                return new string(value[0], repeatCount);
            switch (repeatCount)
            {
                case 1:
                    return value;

                case 2:
                    return string.Concat(value, value);

                case 3:
                    return string.Concat(value, value, value);

                case 4:
                    return string.Concat(value, value, value, value);
            }
            var sb = new StringBuilder(value.Length * repeatCount);
            while (repeatCount-- > 0)
                sb.Append(value);
            return sb.ToString();
        }

        public static string ExtractAround(this string value, int index, int left, int right)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (index >= value.Length)
                throw new IndexOutOfRangeException("参数索引值超出字符串的最大长度");
            var startIndex = Math.Max(0, index - left);
            var length = Math.Min(value.Length - startIndex, index - startIndex + right);
            return value.Substring(startIndex, length);
        }

        public static string ExtractLettersNumbers(this string value) => value.Where(x => !x.IsChinese() && char.IsLetterOrDigit(x))
            .Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c))
            .ToString();

        public static string ExtractNumbers(this string value) => value.Where(char.IsDigit)
            .Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c))
            .ToString();

        public static string ExtractLetters(this string value) => value.Where(x => !x.IsChinese() && char.IsLetter(x))
            .Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c))
            .ToString();

        public static string ExtractChinese(this string value) => value.Where(x => x.IsChinese())
            .Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c))
            .ToString();

        public static string FilterChars(this string value, Predicate<char> predicate) =>
            value.Where(x => predicate(x))
                .Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c))
                .ToString();

        public static string Remove(this string value, params char[] removeChar)
        {
            var result = value;
            if (!string.IsNullOrEmpty(result) && removeChar != null)
            {
                Array.ForEach(removeChar, c => result = result.Remove(c.ToString()));
            }
            return result;
        }

        public static string Remove(this string value, params string[] strings)
        {
            return strings.Aggregate(value, (current, c) => current.Replace(c, string.Empty));
        }

        public static string Remove(this string value, int index, bool isLeft = true)
        {
            if (value.Length <= index)
            {
                return "";
            }
            if (isLeft)
            {
                return value.Substring(index);
            }
            return value.Substring(0, value.Length - index);
        }

        public static string RemoveAllSpecialCharacters(this string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            foreach (var c in value.Where(Char.IsLetterOrDigit))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveEnd(this string value, string defaultChar = ",")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(defaultChar))
            {
                return value.SafeString();
            }

            if (value.ToLower().EndsWith(defaultChar.ToLower()))
            {
                return value.Remove(value.Length - defaultChar.Length, defaultChar.Length);
            }
            return value;
        }

        public static string Remove(this string str, string tag, RegexOptions options = RegexOptions.None)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return tag.IsEmpty() ? str : Regex.Replace(str, tag, "", options);
        }

        public static string RemoveWriteSpace(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
                return src;
            return Regex.Replace(src, @"( |　)+", "");
        }

        public static string FormatWith(this string format, params object[] args)
        {
            format.CheckNotNull("format");
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        public static string ReverseString(this string value)
        {
            value.CheckNotNull("value");
            return new string(value.Reverse().ToArray());
        }

        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.SafeString().Split(new[] { strSplit },
                removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        public static IDictionary<string, string> ToMap(this string delimitedText, char keyValuePairDelimiter,
            char keyValueDelimeter, bool makeKeysCaseSensitive, bool makeValueCaseSensitive, bool trimValues)
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
            string[] tokens = delimitedText.SafeString().Split(keyValuePairDelimiter);

            if (tokens == null) return map;

            foreach (string token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                    continue;
                string[] pair = token.Split(keyValueDelimeter);

                if (pair.Length < 2)
                    continue;

                string key = pair[0];
                string value = pair[1];

                if (makeKeysCaseSensitive)
                {
                    key = key.ToLower();
                }
                if (makeValueCaseSensitive)
                {
                    value = value.ToLower();
                }
                if (trimValues)
                {
                    key = key.Trim();
                    value = value.Trim();
                }
                map[key] = value;
            }
            return map;
        }

        public static int GetTextLength(this string value)
        {
            var ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] bytes = ascii.GetBytes(value);
            foreach (byte b in bytes)
            {
                if (b == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        public static string TrimToMaxLength(this string value, int maxLength)
        {
            return (value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength));
        }

        public static string TrimToMaxLength(this string value, int maxLength, string suffix)
        {
            return (value == null || value.Length <= maxLength ? value : string.Concat(value.Substring(0, maxLength), suffix));
        }

        public static string Truncate(this string txt, int maxChars)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            return txt.Substring(0, maxChars);
        }

        public static string TruncateWithText(this string txt, int maxChars, string suffix)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            string partial = txt.Substring(0, maxChars);
            return partial + suffix;
        }

        public static string TruncateWithText(this string txt, int maxChars)
        {
            if (String.IsNullOrEmpty(txt))
                return txt;

            txt = Regex.Replace(txt, "<[^>]+>", "");
            txt = txt.Trim().Replace("\r\n", "").Replace("\t", "").Replace(" ", "");

            return TruncateWithText(txt, maxChars, "...");
        }

        public static string PadBoth(this string value, int width, char padChar, bool truncate = false)
        {
            int diff = width - value.Length;
            if (diff == 0 || diff < 0 && !(truncate))
            {
                return value;
            }
            else if (diff < 0)
            {
                return value.Substring(0, width);
            }
            else
            {
                return value.PadLeft(width - diff / 2, padChar).PadRight(width, padChar);
            }
        }

        public static string EnsureStartsWith(this string value, string prefix)
        {
            return value.StartsWith(prefix) ? value : string.Concat(prefix, value);
        }

        public static string EnsureEndWith(this string value, string suffix)
        {
            return value.EndsWith(suffix) ? value : string.Concat(value, suffix);
        }

        public static string ConcatWith(this string value, params string[] values)
        {
            return string.Concat(value, string.Concat(values));
        }

        public static string Join<T>(this string value, string separator, T[] obj)
        {
            if (obj == null || obj.Length == 0)
            {
                return value;
            }
            if (separator == null)
            {
                separator = string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(value);
            sb.Append(separator);
            sb.Append(string.Join(separator, Array.ConvertAll(obj, o => o.ToString())));
            return sb.ToString();
        }

        public static string JoinNotNullOrEmpty(this string[] values, string separator)
        {
            var items = values.Where(s => !string.IsNullOrEmpty(s)).ToList();
            return string.Join(separator, items.ToArray());
        }

        public static string GetBefore(this string value, string x)
        {
            var xPos = value.IndexOf(x, StringComparison.Ordinal);
            return xPos == -1 ? string.Empty : value.Substring(0, xPos);
        }

        public static string GetBetween(this string value, string x, string y)
        {
            var xPos = value.IndexOf(x, StringComparison.Ordinal);
            var yPos = value.LastIndexOf(y, StringComparison.Ordinal);
            if (xPos == -1 || yPos == -1)
            {
                return string.Empty;
            }
            var startIndex = xPos + x.Length;
            return startIndex >= yPos ? string.Empty : value.Substring(startIndex, yPos - startIndex).Trim();
        }

        public static string GetAfter(this string value, string x)
        {
            var xPos = value.IndexOf(x, StringComparison.Ordinal);
            if (xPos == -1)
            {
                return string.Empty;
            }
            var startIndex = xPos + x.Length;
            return startIndex >= value.Length ? string.Empty : value.Substring(startIndex).Trim();
        }

        public static string SubstringFrom(this string value, int index)
        {
            return index < 0 && index < value.Length ? value : value.Substring(index, value.Length - index);
        }

        public static string ToUpperFirstLetter(this string value)
        {
            return ToFirstLetter(value);
        }

        public static string ToLowerFirstLetter(this string value)
        {
            return ToFirstLetter(value, false);
        }

        private static string ToFirstLetter(string value, bool isUpper = true)
        {
            if (value.IsEmpty())
            {
                return string.Empty;
            }
            char[] valueChars = value.ToCharArray();
            if (isUpper)
            {
                valueChars[0] = char.ToUpper(valueChars[0]);
            }
            else
            {
                valueChars[0] = char.ToLower(valueChars[0]);
            }
            return new string(valueChars);
        }

        public static string ToTitleCase(this string value)
        {
            return value.ToTitleCase(CultureInfo.CurrentCulture);
        }

        public static string ToTitleCase(this string value, CultureInfo culture)
        {
            return culture.TextInfo.ToTitleCase(value);
        }

        public static string ToPlural(this string singular)
        {
            int index = singular.LastIndexOf(" of ", StringComparison.Ordinal);
            if (index > 0)
            {
                return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();
            }
            if (singular.EndsWith("sh") || singular.EndsWith("ch") || singular.EndsWith("us") || singular.EndsWith("ss"))
            {
                return singular + "es";
            }
            if (singular.EndsWith("y"))
            {
                return singular.Remove(singular.Length - 1, 1) + "ies";
            }
            if (singular.EndsWith("o"))
            {
                return singular.Remove(singular.Length - 1, 1) + "oes";
            }
            return singular + "s";
        }

        public static string ReplaceAll(this string value, IEnumerable<string> oldValues,
            Func<string, string> replacePredicate)
        {
            StringBuilder sb = new StringBuilder(value);
            foreach (var oldValue in oldValues)
            {
                var newValue = replacePredicate(oldValue);
                sb.Replace(oldValue, newValue);
            }
            return sb.ToString();
        }

        public static string ReplaceAll(this string value, IEnumerable<string> oldValues, string newValue)
        {
            StringBuilder sb = new StringBuilder(value);
            foreach (var oldValue in oldValues)
            {
                sb.Replace(oldValue, newValue);
            }
            return sb.ToString();
        }

        public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
        {
            StringBuilder sb = new StringBuilder(value);
            var newValueEnum = newValues.GetEnumerator();
            foreach (var oldValue in oldValues)
            {
                if (!newValueEnum.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("newValues", "newValues sequence is shorter than oldValues sequence");
                }
                sb.Replace(oldValue, newValueEnum.Current);
            }
            if (newValueEnum.MoveNext())
            {
                throw new ArgumentOutOfRangeException("newValues", "newValues sequence is longer than oldValues sequence");
            }
            return sb.ToString();
        }

        public static StringDictionary ParseCommandlineParams(this string[] value)
        {
            var parameters = new StringDictionary();
            var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string parameter = null;
            foreach (string txt in value)
            {
                string[] parts = spliter.Split(txt, 3);
                switch (parts.Length)
                {
                    case 1:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                            {
                                parts[0] = remover.Replace(parts[0], "$1");
                                parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        break;
                    case 2:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;
                    case 3:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        if (!parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            parameters.Add(parameter, parts[2]);
                        }
                        parameter = null;
                        break;
                }
            }
            if (parameter != null)
            {
                if (!parameters.ContainsKey(parameter))
                {
                    parameters.Add(parameter, "true");
                }
            }
            return parameters;
        }

        public static TEnum ParseStringToEnum<TEnum>(this string value, bool ignorecase = default)
            where TEnum : struct
        {
            return value.IsItemInEnum<TEnum>()()
                ? default
                : (TEnum)Enum.Parse(typeof(TEnum), value, ignorecase);
        }

        public static string EncodeEmailAddress(this string emailAddress)
        {
            string tempHtmlEncode = emailAddress;
            for (int i = tempHtmlEncode.Length; i >= 1; i--)
            {
                int acode = Convert.ToInt32(tempHtmlEncode[i - 1]);
                string repl;
                switch (acode)
                {
                    case 32:
                        repl = " ";
                        break;

                    case 34:
                        repl = "\"";
                        break;

                    case 38:
                        repl = "&";
                        break;

                    case 60:
                        repl = "<";
                        break;

                    case 62:
                        repl = ">";
                        break;

                    default:
                        if (acode >= 32 && acode <= 127)
                        {
                            repl = "&#" + Convert.ToString(acode) + ";";
                        }
                        else
                        {
                            repl = "&#" + Convert.ToString(acode) + ";";
                        }
                        break;
                }
                if (repl.Length > 0)
                {
                    tempHtmlEncode = tempHtmlEncode.Substring(0, i - 1) +
                                     repl + tempHtmlEncode.Substring(i);
                }
            }
            return tempHtmlEncode;
        }

        public static string RepairZero(this string text, int limitedLength)
        {
            return text.PadLeft(limitedLength, '0');
        }

        public static string ReplaceFirst(this string @this, string oldValue, string newValue)
        {
            var startIndex = @this.IndexOf(oldValue, StringComparison.Ordinal);
            if (startIndex == -1)
            {
                return @this;
            }

            return @this.Remove(startIndex, oldValue.Length).Insert(startIndex, newValue);
        }

        public static string ReplaceFirst(this string @this, int number, string oldValue, string newValue)
        {
            List<string> list = @this.SafeString().Split(oldValue).ToList();
            var old = number + 1;
            IEnumerable<string> listStart = list.Take(old);
            IEnumerable<string> listEnd = list.Skip(old);

            return string.Join(newValue, listStart)
                   + (listEnd.Any() ? oldValue : "")
                   + string.Join(oldValue, listEnd);
        }

        public static string ReplaceLast(this string @this, string oldValue, string newValue)
        {
            var startIndex = @this.LastIndexOf(oldValue, StringComparison.Ordinal);
            if (startIndex == -1)
            {
                return @this;
            }

            return @this.Remove(startIndex, oldValue.Length).Insert(startIndex, newValue);
        }

        public static string ReplaceLast(this string @this, int number, string oldValue, string newValue)
        {
            List<string> list = @this.SafeString().Split(oldValue).ToList();
            var old = Math.Max(0, list.Count - number - 1);
            IEnumerable<string> listStart = list.Take(old);
            IEnumerable<string> listEnd = list.Skip(old);

            return string.Join(oldValue, listStart)
                   + (old > 0 ? oldValue : "")
                   + string.Join(newValue, listEnd);
        }

        public static string Left(this string value, int length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (length >= value.Length)
                throw new ArgumentOutOfRangeException(nameof(length), length, $"{nameof(length)} 不能大于给定字符串的长度");
            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (length >= value.Length)
                throw new ArgumentOutOfRangeException(nameof(length), length, $"{nameof(length)} 不能大于给定字符串的长度");
            return value.Substring(value.Length - length, length);
        }

        public static int[] ToIntArray(this string delimitedText)
        {
            return ToIntArray(delimitedText, ",");
        }

        public static int[] ToIntArray(this string delimitedText, string delimeter)
        {
            return Array.ConvertAll(Split(delimitedText, delimeter, true), c => c.ToInt());
        }

        public static long[] ToLongArray(this string delimitedText)
        {
            return ToLongArray(delimitedText, ",");
        }

        public static long[] ToLongArray(this string delimitedText, string delimeter)
        {
            return Array.ConvertAll(Split(delimitedText, delimeter, true), c => c.ToLong());
        }

        public static double[] ToDoubleArray(this string delimitedText)
        {
            return ToDoubleArray(delimitedText, ",");
        }

        public static double[] ToDoubleArray(this string delimitedText, string delimeter)
        {
            return Array.ConvertAll(Split(delimitedText, delimeter), c => c.ToDouble());
        }

        public static short[] ToShortArray(this string delimitedText)
        {
            return ToShortArray(delimitedText, ",");
        }

        public static short[] ToShortArray(this string delimitedText, string delimeter)
        {
            return Array.ConvertAll(Split(delimitedText, delimeter), c => c.ToShort());
        }
    }
}