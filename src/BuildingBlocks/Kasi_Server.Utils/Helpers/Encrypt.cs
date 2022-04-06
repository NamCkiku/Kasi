using Kasi_Server.Utils.Extensions;
using Kasi_Server.Utils.Helpers.Internal;
using Kasi_Server.Utils.Serializer;
using System.Security.Cryptography;
using System.Text;

namespace Kasi_Server.Utils.Helpers
{
    public static class Encrypt
    {
        public static string Md5By16(string value)
        {
            return Md5By16(value, Encoding.UTF8);
        }

        public static string Md5By16(string value, Encoding encoding)
        {
            return Md5(value, encoding, 4, 8);
        }

        public static string Md5By32(string value)
        {
            return Md5By32(value, Encoding.UTF8);
        }

        public static string Md5By32(string value, Encoding encoding)
        {
            return Md5(value, encoding, null, null);
        }

        private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var md5 = new MD5CryptoServiceProvider();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex == null
                    ? BitConverter.ToString(hash)
                    : BitConverter.ToString(hash, startIndex.SafeValue(), length.SafeValue());
            }
            finally
            {
                md5?.Clear();
                md5?.Dispose();
            }
            return result.Replace("-", "");
        }

        public static string DesKey = "#s^un2ye21fcv%|f0XpR,+vh";

        public static string DesEncrypt(object value)
        {
            return DesEncrypt(value, DesKey);
        }

        public static string DesEncrypt(object value, string key)
        {
            return DesEncrypt(value, key, Encoding.UTF8);
        }

        public static string DesEncrypt(object value, string key, Encoding encoding)
        {
            var text = value.SafeString();
            if (ValidateDes(text, key) == false)
            {
                return string.Empty;
            }
            using (var transform = CreateDesProvider(key).CreateEncryptor())
            {
                return GetEncryptResult(text, encoding, transform);
            }
        }

        private static bool ValidateDes(string text, string key)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return key.Length == 24;
        }

        private static TripleDESCryptoServiceProvider CreateDesProvider(string key)
        {
            return new TripleDESCryptoServiceProvider()
            {
                Key = Encoding.ASCII.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }

        private static string GetEncryptResult(string value, Encoding encoding, ICryptoTransform transform)
        {
            var bytes = encoding.GetBytes(value);
            var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(result);
        }

        public static string DesDecrypt(object value)
        {
            return DesDecrypt(value, DesKey);
        }

        public static string DesDecrypt(object value, string key)
        {
            return DesDecrypt(value, key, Encoding.UTF8);
        }

        public static string DesDecrypt(object value, string key, Encoding encoding)
        {
            var text = value.SafeString();
            if (!ValidateDes(text, key))
            {
                return string.Empty;
            }

            using (var transform = CreateDesProvider(key).CreateDecryptor())
            {
                return GetDecryptResult(text, encoding, transform);
            }
        }

        private static string GetDecryptResult(string value, Encoding encoding, ICryptoTransform transform)
        {
            var bytes = Convert.FromBase64String(value);
            var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return encoding.GetString(result);
        }

        private static byte[] _iv;

        private static byte[] Iv
        {
            get
            {
                if (_iv == null)
                {
                    var size = 16;
                    _iv = new byte[size];
                    for (int i = 0; i < size; i++)
                    {
                        _iv[i] = 0;
                    }
                }
                return _iv;
            }
        }

        public static string AesKey = "QaP1AF8utIarcBqdhYTZpVGbiNQ9M6IL";

        public static string AesEncrypt(string value)
        {
            return AesEncrypt(value, AesKey);
        }

        public static string AesEncrypt(string value, string key)
        {
            return AesEncrypt(value, key, Encoding.UTF8);
        }

        public static string AesEncrypt(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var rijndaelManaged = CreateRijndaelManaged(key);
            using (var transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV))
            {
                return GetEncryptResult(value, encoding, transform);
            }
        }

        private static RijndaelManaged CreateRijndaelManaged(string key)
        {
            return new RijndaelManaged()
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Iv
            };
        }

        public static string AesDecrypt(string value)
        {
            return AesDecrypt(value, AesKey);
        }

        public static string AesDecrypt(string value, string key)
        {
            return AesDecrypt(value, key, Encoding.UTF8);
        }

        public static string AesDecrypt(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var rijndaelManaged = CreateRijndaelManaged(key);
            using (var transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV))
            {
                return GetDecryptResult(value, encoding, transform);
            }
        }

        public static string AesEncrypt<T>(T input, string appSecret, string appId)
        {
            if (input == null) return string.Empty;

            var json = input.ToJson(camelCase: true);

            return Encrypt.AesEncrypt(json, appSecret, appId);
        }

        public static T AesDecrypt<T>(string input, string appSecret, ref string appId)
        {
            if (string.IsNullOrEmpty(input)) return default;

            var val = Encrypt.AesDecrypt(input, appSecret, ref appId);

            return val.ToObject<T>();
        }

        public static string AesDecrypt(string input, string appSecret, ref string appid)
        {
            byte[] Key = Encoding.UTF8.GetBytes(appSecret);
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            byte[] btmpMsg = AesDecrypt(input, Iv, Key);

            int len = BitConverter.ToInt32(btmpMsg, 0);
            var msgLen = 4;
            byte[] bMsg = new byte[len];
            byte[] bAppid = new byte[btmpMsg.Length - msgLen - len];
            Array.Copy(btmpMsg, msgLen, bMsg, 0, len);
            Array.Copy(btmpMsg, msgLen + len, bAppid, 0, btmpMsg.Length - msgLen - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            appid = Encoding.UTF8.GetString(bAppid);

            return oriMsg;
        }

        private static byte[] AesDecrypt(string input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] baseStr = Convert.FromBase64String(input);

                    byte[] msg = new byte[baseStr.Length + 32 - baseStr.Length % 32];
                    Array.Copy(baseStr, msg, baseStr.Length);

                    cs.Write(baseStr, 0, baseStr.Length);
                }
                xBuff = decode2(ms.ToArray());
            }
            return xBuff;
        }

        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
                pad = 0;
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }

        public static string AesEncrypt(string input, string appSecret, string appid)
        {
            byte[] Key = Encoding.UTF8.GetBytes(appSecret);
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(input);
            byte[] bMsgLen = BitConverter.GetBytes(btmpMsg.Length);
            byte[] bAppid = Encoding.UTF8.GetBytes(appid);
            byte[] bMsg = new byte[bMsgLen.Length + btmpMsg.Length + bAppid.Length];

            Array.Copy(bMsgLen, 0, bMsg, 0, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bAppid, 0, bMsg, bMsgLen.Length + btmpMsg.Length, bAppid.Length);
            return AesEncrypt(bMsg, Iv, Key);
        }

        private static string AesEncrypt(byte[] input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            byte[] msg = new byte[input.Length + 32 - input.Length % 32];
            Array.Copy(input, msg, input.Length);
            byte[] pad = KCS7Encoder(input.Length);
            Array.Copy(pad, 0, msg, input.Length, pad.Length);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }

        private static byte[] KCS7Encoder(int text_length)
        {
            int block_size = 32;
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            char pad_chr = chr(amount_to_pad);
            string tmp = "";
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }

        private static char chr(int a)
        {
            byte target = (byte)(a & 0xFF);
            return (char)target;
        }

        public static string RsaSign(string value, string key)
        {
            return RsaSign(value, key, Encoding.UTF8);
        }

        public static string RsaSign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA);
        }

        public static string Rsa2Sign(string value, string key)
        {
            return Rsa2Sign(value, key, Encoding.UTF8);
        }

        public static string Rsa2Sign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA2);
        }

        private static string RsaSign(string value, string key, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var rsa = new RsaHelper(type, encoding, key);
            return rsa.Sign(value);
        }

        public static bool RsaVerify(string value, string publicKey, string sign)
        {
            return RsaVerify(value, publicKey, sign, Encoding.UTF8);
        }

        public static bool RsaVerify(string value, string publicKey, string sign, Encoding encoding)
        {
            return RsaVerify(value, publicKey, sign, encoding, RSAType.RSA);
        }

        public static bool Rsa2Verify(string value, string publicKey, string sign)
        {
            return Rsa2Verify(value, publicKey, sign, Encoding.UTF8);
        }

        public static bool Rsa2Verify(string value, string publicKey, string sign, Encoding encoding)
        {
            return RsaVerify(value, publicKey, sign, encoding, RSAType.RSA2);
        }

        private static bool RsaVerify(string value, string publicKey, string sign, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            var rsa = new RsaHelper(type, encoding, publicKey: publicKey);
            return rsa.Verify(value, sign);
        }

        public static string HmacMd5(string value, string key)
        {
            return HmacMd5(value, key, Encoding.UTF8);
        }

        public static string HmacMd5(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var md5 = new HMACMD5(encoding.GetBytes(key));
            var hash = md5.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        public static string HmacSha1(string value, string key)
        {
            return HmacSha1(value, key, Encoding.UTF8);
        }

        public static string HmacSha1(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha1 = new HMACSHA1(encoding.GetBytes(key));
            var hash = sha1.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        public static string HmacSha256(string value, string key)
        {
            return HmacSha256(value, key, Encoding.UTF8);
        }

        public static string HmacSha256(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha256 = new HMACSHA256(encoding.GetBytes(key));
            var hash = sha256.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        public static string HmacSha384(string value, string key)
        {
            return HmacSha384(value, key, Encoding.UTF8);
        }

        public static string HmacSha384(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha384 = new HMACSHA384(encoding.GetBytes(key));
            var hash = sha384.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        public static string HmacSha512(string value, string key)
        {
            return HmacSha512(value, key, Encoding.UTF8);
        }

        public static string HmacSha512(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha512 = new HMACSHA512(encoding.GetBytes(key));
            var hash = sha512.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        public static string Sha1(string value)
        {
            return Sha1(value, Encoding.UTF8);
        }

        public static string Sha1(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(encoding.GetBytes(value));
                return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
            }
        }

        public static string Sha256(string value)
        {
            return Sha256(value, Encoding.UTF8);
        }

        public static string Sha256(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(encoding.GetBytes(value));
                return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
            }
        }

        public static string Sha384(string value)
        {
            return Sha384(value, Encoding.UTF8);
        }

        public static string Sha384(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using (var sha = SHA384.Create())
            {
                var hash = sha.ComputeHash(encoding.GetBytes(value));
                return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
            }
        }

        public static string Sha512(string value)
        {
            return Sha512(value, Encoding.UTF8);
        }

        public static string Sha512(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using (var sha = SHA512.Create())
            {
                var hash = sha.ComputeHash(encoding.GetBytes(value));
                return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
            }
        }

        public static string Base64Encrypt(string value)
        {
            return Base64Encrypt(value, Encoding.UTF8);
        }

        public static string Base64Encrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static string Base64Decrypt(string value)
        {
            return Base64Decrypt(value, Encoding.UTF8);
        }

        public static string Base64Decrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : encoding.GetString(Convert.FromBase64String(value));
        }
    }
}