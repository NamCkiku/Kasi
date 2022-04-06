using Kasi_Server.Utils.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Helpers
{
    public static class Valid
    {
        public static bool IsEmail(string value, bool isRestrict = false)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string pattern = isRestrict
                ? @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"
                : @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";

            return value.IsMatch(pattern, RegexOptions.IgnoreCase);
        }

        public static bool HasEmail(string value, bool isRestrict = false)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string pattern = isRestrict
                ? @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"
                : @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsPhoneNumber(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[678])[0-9]{8}$");
        }

        public static bool IsMobileNumberSimple(string value, bool isRestrict = false)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        public static bool IsMobileNumber(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            value = value.Trim().Replace("^", "").Replace("$", "");
            return value.IsMatch(@"^1(3[0-9]|4[57]|5[0-35-9]|8[0-9]|70)\d{8}$");
        }

        public static bool HasMobileNumberSimple(string value, bool isRestrict = false)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string pattern = isRestrict ? @"[1][3-8]\d{9}" : @"[1]\d{10}";
            return value.IsMatch(pattern);
        }

        public static bool IsChinaMobilePhone(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }

            return value.IsMatch(@"(^1(3[4-9]|4[7]|5[0-27-9]|7[8]|8[2-478])\d{8}$)|(^1705\d{7}$)");
        }

        public static bool IsChinaUnicomPhone(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"(^1(3[0-2]|4[5]|5[56]|7[6]|8[56])\d{8}$)|(^1709\d{7}$)");
        }

        public static bool IsChinaTelecomPhone(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"(^1(33|53|77|8[019])\d{8}$)|(^1700\d{7}$)");
        }

        public static bool IsTel(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^\d{3,4}-?\d{6,8}$", RegexOptions.IgnoreCase);
        }

        public static bool IsIdCard(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            if (value.Length == 15)
            {
                return value.IsMatch(@"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            return value.Length == 0x12 &&
                   value.IsMatch(@"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$",
                       RegexOptions.IgnoreCase);
        }

        public static bool IsBase64String(string value)
        {
            return value.IsMatch(@"[A-Za-z0-9\+\/\=]");
        }

        public static bool IsGuid(string guid)
        {
            if (guid.IsEmpty())
            {
                return false;
            }
            return guid.IsMatch(@"[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }

        public static bool IsUrl(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return
                value.IsMatch(
                    @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$",
                    RegexOptions.IgnoreCase);
        }

        public static bool IsUri(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            if (value.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }
            var schemes = new[]
            {
                "file",
                "ftp",
                "gopher",
                "http",
                "https",
                "ldap",
                "mailto",
                "net.pipe",
                "net.tcp",
                "news",
                "nntp",
                "telnet",
                "uuid"
            };

            bool hasValidSchema = false;
            foreach (string scheme in schemes)
            {
                if (hasValidSchema)
                {
                    continue;
                }
                if (value.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    hasValidSchema = true;
                }
            }
            if (!hasValidSchema)
            {
                value = "http://" + value;
            }
            return Uri.IsWellFormedUriString(value, UriKind.Absolute);
        }

        public static bool IsMainDomainUrl(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return
                value.IsMatch(
                    @"^http(s)?\://((www.)?[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        public static bool IsMainDomain(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(
                @"^((www.)?[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*$",
                RegexOptions.IgnoreCase);
        }

        public static bool IsDomain(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(
                @"^(([a-zA-Z0-9\-]+\.)?[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*$",
                RegexOptions.IgnoreCase);
        }

        public static bool IsMac(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^([0-9A-F]{2}-){5}[0-9A-F]{2}$") || value.IsMatch(@"^[0-9A-F]{12}$");
        }

        public static bool IsPositiveInteger(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[1-9]+\d*$");
        }

        public static bool IsInt32(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[0-9]*$");
        }

        public static bool IsDouble(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^\d[.]?\d?$");
        }

        public static bool IsDouble(string value, double minValue, double maxValue, int digit)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string patten = string.Format(@"^\d[.]?\d{0}$", "{0,10}");
            if (digit > 0)
            {
                patten = string.Format(@"^\d[.]?\d{0}$", "{" + digit + "}");
            }
            if (value.IsMatch(patten))
            {
                double val = Convert.ToDouble(value);
                if (val >= minValue && val <= maxValue)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPasswordOne(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~_]{6,25}$");
        }

        public static bool IsPasswordOne(string value, int min, int max)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(string.Format(@"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~_]{0},{1}$", "{" + min, max + "}"));
        }

        public static bool IsPasswordTwo(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return
                value.IsMatch(
                    @"(?=^.{6,25}$)(?=(?:.*?\d){1})(?=.*[a-z])(?=(?:.*?[A-Z]){1})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$");
        }

        public static bool IsLoginName(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^(?![^a-zA-Z]+$)[A-Za-z0-9]{6,30}$");
        }

        public static bool IsLoginName(string value, int min, int max)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(string.Format(@"^(?![^a-zA-Z]+$)[A-Za-z0-9]{0},{1}$", "{" + min, max + "}"));
        }

        public static bool IsBandCard(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^(\d{16}|\d{19})$");
        }

        public static bool IsSafeSqlString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.IsMatch(@"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']") || value.IsMatch(
                    @"select|insert|delete|from|count(|drop table|update|truncate|asc(|mid(|Char(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and")
            )
            {
                return true;
            }

            return false;
        }

        public static bool IsVersion(string value, int length = 5)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            value = value.Replace("^", "").Replace("$", "");
            return value.IsMatch(string.Format(@"^{0}{1}{2}$", @"\d{0,4}\.(\d{1,4}\.){0,", length, @"}\d{1,4}"));
        }

        public static bool IsChineseWord(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"^[\u4e00-\u9fa5]{0,}$");
        }

        public static bool IsChinese(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
        }

        public static bool IsContainsChinese(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase);
        }

        public static bool IsContainsNumber(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"[0-9]+");
        }

        public static bool IsIpAddress(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$");
        }

        public static bool IsInteger(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^\-?[0-9]+$");
        }

        public static bool IsUnicode(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return
                value.IsMatch(
                    @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$");
        }

        public static bool IsLengthStr(string value, int begin, int end)
        {
            int length = Regex.Replace(value, @"[^\x00-\xff]", "OK").Length;
            if ((length <= begin) && (length >= end))
            {
                return false;
            }
            return true;
        }

        public static bool IsChinesePostalCode(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[1-9]\d{5}$", RegexOptions.IgnoreCase);
        }

        public static bool IsNormalChar(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"[\w\d_]+", RegexOptions.IgnoreCase);
        }

        public static bool IsPostfix(string value, string[] postfixs)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            string postfix = string.Join("|", postfixs);
            return value.IsMatch(string.Format(@".(?i:{0})$", postfix));
        }

        public static bool IsDecimal(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^([0-9])[0-9]*(\.\w*)?$");
        }

        public static bool IsRepeat(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            var array = value.ToCharArray();
            return array.Any(c => array.Count(t => t == c) > 1);
        }

        public static bool IsQQ(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"^[1-9][0-9]{4,9}$");
        }

        public static bool IsColorValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim().Trim('#');
            if (value.Length != 33 && value.Length != 6)
            {
                return false;
            }

            return !value.IsMatch(@"[^0-9a-f]", RegexOptions.IgnoreCase);
        }

        public static bool IsWideWord(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"[^/x00-/xff]");
        }

        public static bool IsNarrowWord(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"[/x00-/xff]");
        }

        public static bool IsOnlyNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"^\d+$");
        }

        public static bool IsUpperCaseChar(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"^[A-Z]+$");
        }

        public static bool IsUpperCaseChar(char value)
        {
            return IsUpperCaseChar(value.ToString());
        }

        public static bool IsLowerCaseChar(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"^[a-z]+$");
        }

        public static bool IsLowerCaseChar(char value)
        {
            return IsLowerCaseChar(value.ToString());
        }

        public static bool IsNumber(string input)
        {
            if (input.IsEmpty())
            {
                return false;
            }
            const string pattern = @"^(-?\d*)(\.\d+)?$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IsMatch(@"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        public static bool IsDate(string value, bool isRegex = false)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            if (isRegex)
            {
                return
                    value.IsMatch(
                        @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            }
            return DateTime.TryParse(value, out var minValue);
        }

        public static bool IsDate(string value, string format)
        {
            return IsDate(value, format, null, DateTimeStyles.None);
        }

        public static bool IsDate(string value, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            if (value.IsEmpty())
            {
                return false;
            }

            return DateTime.TryParseExact(value, format, provider, styles, out var minValue);
        }

        public static bool IsDateTimeMin(string value, DateTime min)
        {
            if (value.IsEmpty())
            {
                return false;
            }

            if (DateTime.TryParse(value, out var dateTime))
            {
                if (DateTime.Compare(dateTime, min) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsDateTimeMax(string value, DateTime max)
        {
            if (value.IsEmpty())
            {
                return false;
            }

            if (DateTime.TryParse(value, out var dateTime))
            {
                if (DateTime.Compare(max, dateTime) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}