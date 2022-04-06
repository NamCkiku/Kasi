using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder TrimStart(this StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }
            return sb.TrimStart(' ');
        }

        public static StringBuilder TrimStart(this StringBuilder sb, char c)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (sb.Length == 0)
            {
                return sb;
            }

            while (c.Equals(sb[0]))
            {
                sb.Remove(0, 1);
            }

            return sb;
        }

        public static StringBuilder TrimStart(this StringBuilder sb, char[] chars)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (chars == null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            return sb.TrimStart(new string(chars));
        }

        public static StringBuilder TrimStart(this StringBuilder sb, string str)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (string.IsNullOrEmpty(str) || sb.Length == 0 || str.Length > sb.Length)
            {
                return sb;
            }

            while (sb.SubString(0, str.Length).Equals(str))
            {
                sb.Remove(0, str.Length);
                if (str.Length > sb.Length)
                {
                    break;
                }
            }

            return sb;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }
            return sb.TrimEnd(' ');
        }

        public static StringBuilder TrimEnd(this StringBuilder sb, char c)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (sb.Length == 0)
            {
                return sb;
            }

            while (c.Equals(sb[sb.Length - 1]))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb, char[] chars)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (chars == null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            return sb.TrimEnd(new string(chars));
        }

        public static StringBuilder TrimEnd(this StringBuilder sb, string str)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (string.IsNullOrEmpty(str) || sb.Length == 0 || str.Length > sb.Length)
            {
                return sb;
            }

            while (sb.SubString(sb.Length - str.Length, str.Length).Equals(str))
            {
                sb.Remove(sb.Length - str.Length, str.Length);
                if (sb.Length < str.Length)
                {
                    break;
                }
            }

            return sb;
        }

        public static StringBuilder Trim(this StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (sb.Length == 0)
            {
                return sb;
            }

            return sb.TrimEnd().TrimStart();
        }

        public static string SubString(this StringBuilder sb, int start, int length)
        {
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            if (start + length > sb.Length)
            {
                throw new IndexOutOfRangeException("超出字符串索引长度");
            }
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = sb[start + i];
            }
            return new string(chars);
        }

        public static StringBuilder AppendLine(this StringBuilder sb, string value, params object[] parameters)
        {
            return sb.AppendLine(string.Format(value, parameters));
        }

        public static StringBuilder AppendJoin<T>(this StringBuilder sb, string separator, params T[] values)
        {
            sb.Append(string.Join(separator, values));
            return sb;
        }

        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition)
            {
                sb.Append(value.ToString());
            }

            return sb;
        }

        public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string value,
            params object[] parameters)
        {
            if (condition)
            {
                sb.AppendFormat(value, parameters);
            }

            return sb;
        }

        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition)
            {
                sb.AppendLine(value.ToString());
            }

            return sb;
        }

        public static StringBuilder AppendLine(this StringBuilder sb, bool condition, string value,
            params object[] parmaeters)
        {
            if (condition)
            {
                sb.AppendFormat(value, parmaeters).AppendLine();
            }

            return sb;
        }
    }
}