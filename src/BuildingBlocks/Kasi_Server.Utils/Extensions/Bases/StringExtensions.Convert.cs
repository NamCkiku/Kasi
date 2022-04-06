using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            encoding = (encoding ?? Encoding.UTF8);
            return encoding.GetBytes(value);
        }

        public static XDocument ToXDocument(this string xml)
        {
            return XDocument.Parse(xml);
        }

        public static XElement ToXElement(this string xml)
        {
            return XElement.Parse(xml);
        }

        public static XmlDocument ToXmlDocument(this string xml)
        {
            var documnet = new XmlDocument();
            documnet.LoadXml(xml);
            return documnet;
        }

        public static XPathNavigator ToXPath(this string xml)
        {
            var documnet = new XPathDocument(new StringReader(xml));
            return documnet.CreateNavigator();
        }

        public static byte[] HexStringToBytes(this string value)
        {
            value = value.Replace(" ", "");
            int maxByte = value.Length / 2 - 1;
            var bytes = new byte[maxByte + 1];
            for (int i = 0; i <= maxByte; i++)
            {
                bytes[i] = byte.Parse(value.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            }
            return bytes;
        }

        public static string ToUnicodeString(this string source)
        {
            string outString = "";
            if (!string.IsNullOrEmpty(source))
            {
                outString = source.Aggregate(outString, (current, t) => current + (@"\u" + ((int)t).ToString("x").ToUpper()));
            }
            return outString;
        }

        public static SecureString ToSecureString(this string value, bool markReadOnly = true)
        {
            if (value.IsEmpty())
            {
                return null;
            }
            SecureString temp = new SecureString();
            foreach (char c in value)
            {
                temp.AppendChar(c);
            }
            if (markReadOnly)
            {
                temp.MakeReadOnly();
            }
            return temp;
        }

        public static string ToUnSecureString(this SecureString value)
        {
            if (ReferenceEquals(value, null))
            {
                return null;
            }
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static string ToSbcCase(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }

        public static string ToDbcCase(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 35280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        public static DateTime ToDateTime(this string timeStamp)
        {
            if (timeStamp.Length > 10)
            {
                timeStamp = timeStamp.Substring(0, 10);
            }
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lIime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lIime);
            return dateTimeStart.Add(toNow);
        }
    }
}