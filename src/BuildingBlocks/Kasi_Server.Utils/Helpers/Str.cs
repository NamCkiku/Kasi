using Kasi_Server.Utils.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Helpers
{
    public partial class Str
    {
        public static string Join<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            if (list == null)
            {
                return string.Empty;
            }
            var result = new StringBuilder();
            foreach (var each in list)
            {
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            }
            if (separator == "")
            {
                return result.ToString();
            }
            return result.ToString().TrimEnd(separator.ToCharArray());
        }

        public static string ToUnicode(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                sb.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'),
                    bytes[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        public static string ToUnicodeByCn(string value)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(value))
            {
                char[] chars = value.ToCharArray();
                for (int i = 0; i < value.Length; i++)
                {
                    sb.Append(Regex.IsMatch(chars[i].ToString(), "([\u4e00-\u9fa5])")
                        ? ToUnicode(chars[i].ToString())
                        : chars[i].ToString());
                }
            }

            return sb.ToString();
        }

        public static string UnicodeToStr(string value)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(value,
                x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }

        public static string PinYin(string chineseText)
        {
            if (string.IsNullOrWhiteSpace(chineseText))
            {
                return string.Empty;
            }
            var result = new StringBuilder();
            foreach (var text in chineseText)
            {
                result.AppendFormat("{0}", ResolvePinYin(text));
            }

            return result.ToString().ToLower();
        }

        private static string ResolvePinYin(char text)
        {
            byte[] charBytes = Encoding.Default.GetBytes(text.ToString());
            if (charBytes[0] < 127)
            {
                return text.ToString();
            }
            var unicode = (ushort)(charBytes[0] * 256 + charBytes[1]);
            string pinYin = ResolveByCode(unicode);
            if (!string.IsNullOrWhiteSpace(pinYin))
            {
                return pinYin;
            }
            return ResolveByConst(text.ToString());
        }

        private static string ResolveByCode(ushort unicode)
        {
            if (unicode >= '\uB0A1' && unicode <= '\uB0C4')
            {
                return "A";
            }
            if (unicode >= '\uB0C5' && unicode <= '\uB2C0' && unicode != 45464)
            {
                return "B";
            }
            if (unicode >= '\uB2C1' && unicode <= '\uB4ED')
            {
                return "C";
            }
            if (unicode >= '\uB4EE' && unicode <= '\uB6E9')
            {
                return "D";
            }
            if (unicode >= '\uB6EA' && unicode <= '\uB7A1')
            {
                return "E";
            }
            if (unicode >= '\uB7A2' && unicode <= '\uB8C0')
            {
                return "F";
            }
            if (unicode >= '\uB8C1' && unicode <= '\uB9FD')
            {
                return "G";
            }
            if (unicode >= '\uB9FE' && unicode <= '\uBBF6')
            {
                return "H";
            }
            if (unicode >= '\uBBF7' && unicode <= '\uBFA5')
            {
                return "J";
            }
            if (unicode >= '\uBFA6' && unicode <= '\uC0AB')
            {
                return "K";
            }
            if (unicode >= '\uC0AC' && unicode <= '\uC2E7')
            {
                return "L";
            }
            if (unicode >= '\uC2E8' && unicode <= '\uC4C2')
            {
                return "M";
            }
            if (unicode >= '\uC4C3' && unicode <= '\uC5B5')
            {
                return "N";
            }
            if (unicode >= '\uC5B6' && unicode <= '\uC5BD')
            {
                return "O";
            }
            if (unicode >= '\uC5BE' && unicode <= '\uC6D9')
            {
                return "P";
            }
            if (unicode >= '\uC6DA' && unicode <= '\uC8BA')
            {
                return "Q";
            }
            if (unicode >= '\uC8BB' && unicode <= '\uC8F5')
            {
                return "R";
            }
            if (unicode >= '\uC8F6' && unicode <= '\uCBF9')
            {
                return "S";
            }
            if (unicode >= '\uCBFA' && unicode <= '\uCDD9')
            {
                return "T";
            }
            if (unicode >= '\uCDDA' && unicode <= '\uCEF3')
            {
                return "W";
            }
            if (unicode >= '\uCEF4' && unicode <= '\uD188')
            {
                return "X";
            }
            if (unicode >= '\uD1B9' && unicode <= '\uD4D0')
            {
                return "Y";
            }
            if (unicode >= '\uD4D1' && unicode <= '\uD7F9')
            {
                return "Z";
            }
            return string.Empty;
        }

        private static string ResolveByConst(string text)
        {
            int index = Const.ChinesePinYin.IndexOf(text, StringComparison.Ordinal);
            if (index < 0)
            {
                return string.Empty;
            }

            return Const.ChinesePinYin.Substring(index + 1, 1);
        }

        public static string FullPinYin(string text)
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] nowChar = text.ToCharArray();
            for (int j = 0; j < nowChar.Length; j++)
            {
                if (regex.IsMatch(nowChar[j].ToString()))
                {
                    array = Encoding.Default.GetBytes(nowChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += nowChar[j];
                    }
                    else
                    {
                        switch (chrAsc)
                        {
                            case -9254:
                                pyString += "Zhen"; break;
                            case -8985:
                                pyString += "Qian"; break;
                            case -5463:
                                pyString += "Jia"; break;
                            case -8274:
                                pyString += "Ge"; break;
                            case -5448:
                                pyString += "Ga"; break;
                            case -5447:
                                pyString += "La"; break;
                            case -4649:
                                pyString += "Chen"; break;
                            case -5436:
                                pyString += "Mao"; break;
                            case -5213:
                                pyString += "Mao"; break;
                            case -3597:
                                pyString += "Die"; break;
                            case -5659:
                                pyString += "Tian"; break;
                            default:
                                for (int i = (Const.SpellCode.Length - 1); i >= 0; i--)
                                {
                                    if (Const.SpellCode[i] <= chrAsc)
                                    {
                                        pyString += Const.SpellLetter[j];
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    pyString += nowChar[j].ToString();
                }
            }
            return pyString;
        }

        public static string FirstLower(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return $"{value.Substring(0, 1).ToLower()}{value.Substring(1)}";
        }

        public static string FirstUpper(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return $"{value.Substring(0, 1).ToUpper()}{value.Substring(1)}";
        }

        public static string Empty => string.Empty;

        public static string Distinct(string value)
        {
            var array = value.ToCharArray();
            return new string(array.Distinct().ToArray());
        }

        public static string Truncate(string text, int length, int endChatCount = 0, string endChar = ".")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            if (text.Length < length)
            {
                return text;
            }
            return $"{text.Substring(0, length)}{GetEndString(endChatCount, endChar)}";
        }

        private static string GetEndString(int endCharCount, string endChar)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < endCharCount; i++)
            {
                sb.Append(endChar);
            }
            return sb.ToString();
        }

        public static string GetLastProperty(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return string.Empty;
            var lastIndex = propertyName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            return propertyName.Substring(lastIndex);
        }

        public static string GetHideMobile(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return $"{value.Substring(0, 3)}******{value.Substring(value.Length - 3)}";
        }

        public static int GetStringLength(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            int strLength = 0;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(value);
            for (var i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63)
                {
                    strLength++;
                }
                strLength++;
            }
            return strLength;
        }

        public static string ToSnakeCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var sb = new StringBuilder();
            var state = SnakeCaseState.Start;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ')
                {
                    if (state != SnakeCaseState.Start)
                    {
                        state = SnakeCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(str[i]))
                {
                    switch (state)
                    {
                        case SnakeCaseState.Upper:
                            bool hasNext = (i + 1 < str.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = str[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != '_')
                                {
                                    sb.Append('_');
                                }
                            }
                            break;

                        case SnakeCaseState.Lower:
                        case SnakeCaseState.NewWord:
                            sb.Append('_');
                            break;
                    }

                    sb.Append(char.ToLowerInvariant(str[i]));
                    state = SnakeCaseState.Upper;
                }
                else if (str[i] == '_')
                {
                    sb.Append('_');
                    state = SnakeCaseState.Start;
                }
                else
                {
                    if (state == SnakeCaseState.NewWord)
                    {
                        sb.Append('_');
                    }

                    sb.Append(str[i]);
                    state = SnakeCaseState.Lower;
                }
            }

            return sb.ToString();
        }

        public static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }

            char[] chars = str.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = char.ToLowerInvariant(chars[i]);
                    }
                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
            return new string(chars);
        }

        public static string SplitWordGroup(string value, char separator = '-')
        {
            var pattern = @"([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)";
            return string.IsNullOrWhiteSpace(value) ? string.Empty : System.Text.RegularExpressions.Regex.Replace(value, pattern, $"{separator}$1$2").TrimStart(separator).ToLower();
        }

        public static bool IsStrictIDNumber(string src)
        {
            if (src.Length == 15)
            {
                return checkIDNumber15(src);
            }
            else if (src.Length == 18)
            {
                return checkIDNumber18(src);
            }
            return false;
        }

        private static bool checkIDNumber15(string src)
        {
            long n = 0;
            if (long.TryParse(src, out n) == false || n < Math.Pow(10, 14))
            {
                return false;
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2)) == -1)
            {
                return false;
            }
            string birth = src.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;
            }
            return true;
        }

        private static bool checkIDNumber18(string src)
        {
            long n = 0;
            if (long.TryParse(src.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(src.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2)) == -1)
            {
                return false;
            }
            string birth = src.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = src.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != src.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
        }

        public static string ConvertToDialLink(string source)
        {
            Regex reg = new Regex(@"(0{1}[1-9]{1}[0-9]{1,2}\-[1-9]{1}[0-9]{6,7}|\(0{1}[1-9]{1}[0-9]{1,2}\)[1-9]{1}[0-9]{6,7}|0{1}[1-9]{1}[0-9]{1,2}[1-9]{1}[0-9]{6,7}|400[0123456789-]+|1[3584]{1}[0-9]{9})(?=\D|$)?");
            MatchCollection collect = reg.Matches(source);
            if (collect != null)
            {
                foreach (Match m in collect)
                {
                    string number = m.ToString();
                    string link = string.Format("<a href=\"tel:{0}\">{1}</a>", Regex.Replace(number, @"\D", ""), number);
                    source = source.Replace(number, link);
                }
            }
            return source;
        }

        public static string FormatAsLicensePlate(string src)
        {
            if (string.IsNullOrWhiteSpace(src) || string.IsNullOrEmpty(src))
                return src;
            src = src.RemoveWriteSpace().ToUpper();
            if (src.Length > 2)
                src = src.Insert(2, " ");
            return src;
        }

        public static bool IsStrictLicensePlate(string src)
        {
            return Regex.IsMatch(src, @"^[\u4E00-\u9FA5]{1}[A-Z]{1} ?[a-zA-Z0-9]{5}$");
        }

        public static bool IsBankCard(string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return false;
            src = Regex.Replace(src, @" ", "");
            string code = src.Substring(0, src.Length - 1);
            if (!Regex.IsMatch(code, @"^\d+$")) return false;
            char[] chs = code.ToCharArray();
            int luhmSum = 0;
            for (int i = chs.Length - 1, j = 0; i >= 0; i--, j++)
            {
                int k = chs[i] - '0';
                if (j % 2 == 0)
                {
                    k *= 2;
                    k = k / 10 + k % 10;
                }
                luhmSum += k;
            }
            char r = (luhmSum % 10 == 0) ? '0' : (char)((10 - luhmSum % 10) + '0');
            return r.ToString() == src.Substring(src.Length - 1, 1);
        }

        public static List<string> GetImgUrl(string html)
        {
            return GetRegexTagName(html, @"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", "src");
        }

        public static bool IsMatchImages(string html)
        {
            Regex r = new Regex(@"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", RegexOptions.IgnoreCase);
            return r.IsMatch(html);
        }

        public static List<string> GetRegexTagName(string html, string regstr, string keyname)
        {
            List<string> resultStr = new List<string>();
            Regex r = new Regex(regstr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);

            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups[keyname].Value);
            }
            return resultStr;
        }

        public static string GetNonceStrByGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GetNonceStr(int length, bool isUpperCase = true, bool isLowerCase = true, bool isNumbers = true, bool isCharacter = true)
        {
            string str = "";
            if (isUpperCase)
                str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (isLowerCase)
                str += "abcdefghijklmnopqrstuvwxyz";
            if (isNumbers)
                str += "0123456789";
            if (isCharacter)
                str += "!@#$%^&*";
            return GetRandomStr(str, length);
        }

        public static string GetRandomStr(string str, int length)
        {
            Random r = new Random();
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int m = r.Next(0, str.Length);
                string s = str.Substring(m, 1);
                result += s;
            }

            return result;
        }

        public static string CreateOrderNumber(string name, int randomLength = 4)
        {
            return string.Format("{0}{1}{2}", name, DateTime.Now.ToString("yyyyMMddHHmmssfff"), GetNonceStr(randomLength, false, false, true, false));
        }
    }

    public enum StringCase
    {
        Snake,

        Camel,

        None,
    }

    internal enum SnakeCaseState
    {
        Start,

        Lower,

        Upper,

        NewWord
    }
}