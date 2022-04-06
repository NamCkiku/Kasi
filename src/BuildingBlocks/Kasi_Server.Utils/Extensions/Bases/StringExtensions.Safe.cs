using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static string UrlEncode(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return HttpUtility.UrlEncode(source, encoding);
        }

        public static string UrlDecode(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return HttpUtility.UrlDecode(source, encoding);
        }

        public static string ToHtmlSafe(this string value)
        {
            return value.ToHtmlSafe(false, false);
        }

        public static string ToHtmlSafe(this string value, bool all)
        {
            return value.ToHtmlSafe(all, false);
        }

        public static string ToHtmlSafe(this string value, bool all, bool replace)
        {
            if (value.IsEmpty())
            {
                return string.Empty;
            }
            var entities = new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 29,
                30, 31, 34, 39, 38, 60, 62, 123, 124, 125, 126, 127, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169,
                170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190,
                191, 215, 247, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209,
                210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230,
                231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251,
                252, 253, 254, 255, 256, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730,
                8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834,
                8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923,
                924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952,
                953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 338,
                339, 352, 353, 376, 402, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207, 8211, 8212, 8216, 8217,
                8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8364, 8482, 8592,
                8593, 8594, 8595, 8596, 8629, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830
            };
            StringBuilder sb = new StringBuilder();
            foreach (var item in value)
            {
                if (all || entities.Contains(item))
                {
                    sb.Append("&#" + ((int)item) + ";");
                }
                else
                {
                    sb.Append(item);
                }
            }
            return replace
                ? sb.Replace("", "<br />").Replace("\n", "<br />").Replace(" ", "&nbsp;").ToString()
                : sb.ToString();
        }

        public static string EncodeBase64(this string value, Encoding encoding = null)
        {
            encoding = (encoding ?? Encoding.UTF8);
            var bytes = encoding.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(this string value, Encoding encoding = null)
        {
            encoding = (encoding ?? Encoding.UTF8);
            var bytes = Convert.FromBase64String(value);
            return encoding.GetString(bytes);
        }

        public static byte[] EncryptToBytes(this string value, string pwd)
        {
            var asciiEncoder = new ASCIIEncoding();
            byte[] bytes = asciiEncoder.GetBytes(value);
            return CryptBytes(pwd, bytes, true);
        }

        private static byte[] CryptBytes(string pwd, byte[] bytes, bool encrypt)
        {
            var desProvider = new TripleDESCryptoServiceProvider();
            int keySizeBits = 0;
            for (int i = 1024; i >= 1; i--)
            {
                if (desProvider.ValidKeySize(i))
                {
                    keySizeBits = i;
                    break;
                }
            }
            int blockSizeBits = desProvider.BlockSize;
            byte[] key = null;
            byte[] iv = null;
            byte[] salt =
            {
                0x10, 0x20, 0x12, 0x23, 0x37, 0xA4, 0xC5, 0xA6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45
            };
            MakeKeyAndIv(pwd, salt, keySizeBits, blockSizeBits, ref key, ref iv);
            ICryptoTransform cryptoTransform = encrypt
                ? desProvider.CreateEncryptor(key, iv)
                : desProvider.CreateDecryptor(key, iv);
            var outStream = new MemoryStream();
            var cryptoStream = new CryptoStream(outStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            try
            {
                cryptoStream.FlushFinalBlock();
            }
            catch (CryptographicException)
            {
            }
            byte[] result = outStream.ToArray();
            try
            {
                cryptoStream.Close();
            }
            catch (CryptographicException)
            {
            }
            outStream.Close();
            return result;
        }

        private static void MakeKeyAndIv(string pwd, byte[] salt, int keySizeBits, int blockSizeBits, ref byte[] key,
            ref byte[] iv)
        {
            var deriveBytes = new Rfc2898DeriveBytes(pwd, salt, 1234);
            key = deriveBytes.GetBytes(keySizeBits / 8);
            iv = deriveBytes.GetBytes(blockSizeBits / 8);
        }

        public static string DecryptFromBytes(this byte[] value, string pwd)
        {
            byte[] bytes = CryptBytes(pwd, value, false);
            var asciiEncoder = new ASCIIEncoding();
            return new string(asciiEncoder.GetChars(bytes));
        }

        public static string EncryptToString(this string value, string pwd)
        {
            return value.EncryptToBytes(pwd).ToString();
        }

        public static string DecryptFromString(this string value, string pwd)
        {
            var asciiEncoder = new ASCIIEncoding();
            byte[] bytes = asciiEncoder.GetBytes(value);
            return DecryptFromBytes(bytes, pwd);
        }

        public static string TextHTMLEncode(this string strSourceText)
        {
            if (string.IsNullOrEmpty(strSourceText))
                return strSourceText;
            string tmpReturnHTML = strSourceText;
            tmpReturnHTML = tmpReturnHTML.Replace("&", "&#38");
            tmpReturnHTML = tmpReturnHTML.Replace("'", "&#39");
            tmpReturnHTML = tmpReturnHTML.Replace("\"", "&#34");
            tmpReturnHTML = tmpReturnHTML.Replace("<", "&#60");
            tmpReturnHTML = tmpReturnHTML.Replace(">", "&#62");
            tmpReturnHTML = tmpReturnHTML.Replace(" ", "&nbsp;");
            tmpReturnHTML = tmpReturnHTML.Replace("\n", "<br/>");
            tmpReturnHTML = tmpReturnHTML.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            return tmpReturnHTML;
        }

        public static string TextHTMLDecode(this string strSourceText)
        {
            if (string.IsNullOrEmpty(strSourceText))
                return strSourceText;
            string tmpReturnHTML = strSourceText;
            tmpReturnHTML = tmpReturnHTML.Replace("&#38", "&");
            tmpReturnHTML = tmpReturnHTML.Replace("&#39", "'");
            tmpReturnHTML = tmpReturnHTML.Replace("&#34", "\"");
            tmpReturnHTML = tmpReturnHTML.Replace("&#60", "<");
            tmpReturnHTML = tmpReturnHTML.Replace("&#62", ">");
            tmpReturnHTML = tmpReturnHTML.Replace("&nbsp;", " ");
            tmpReturnHTML = tmpReturnHTML.Replace("<br/>", "\n");
            tmpReturnHTML = tmpReturnHTML.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");
            return tmpReturnHTML;
        }

        public static string HtmlFilter(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = Regex.Replace(value, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"-->", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"<!--.*", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            return value;
        }

        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string SqlFilter(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            str = str.Replace("'", "''");
            str = str.Replace(";", "；");
            str = str.Replace("(", "（");
            str = str.Replace(")", "）");
            str = str.Replace("Exec", "");
            str = str.Replace("Execute", "");
            str = str.Replace("xp_", "x p_");
            str = str.Replace("sp_", "s p_");
            str = str.Replace("0x", "0 x");
            return str;
        }
    }
}