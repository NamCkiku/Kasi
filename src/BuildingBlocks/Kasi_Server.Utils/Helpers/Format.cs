namespace Kasi_Server.Utils.Helpers
{
    public static class Format
    {
        public static string EncryptPlateNumberOfChina(string plateNumber, char specialChar = '*') => EncryptString(plateNumber, 2, 2, specialChar);

        public static string EncryptVinCode(string vinCode, char specialChar = '*') => EncryptString(vinCode, 3, 3, specialChar);

        public static string FormatMoney(decimal money, bool isEncrypt = false) => isEncrypt ? "***" : $"{money:N2}";

        public static string EncryptPhone(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (Regexs.IsMatch(value, RegexPatterns.MobilePhone))
                return EncryptString(value, 3, 4, specialChar);

            return EncryptSensitiveInfo(value, specialChar);
        }

        public static string EncryptEmail(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (Regexs.IsMatch(value, RegexPatterns.Email))
            {
                int suffixLen = value.LastIndexOf('@');
                return $"{EncryptSensitiveInfo(value.Substring(0, suffixLen), specialChar)}{value.Substring(suffixLen)}";
            }

            return EncryptSensitiveInfo(value, specialChar);
        }

        public static string EncryptSensitiveInfo(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            var len = value.Length;

            if (len == 1)
                return value;
            else if (len > 1 && len <= 4)
                return EncryptString(value, 1, 0, specialChar);
            else if (len > 4 && len <= 5)
                return EncryptString(value, 1, 1, specialChar);
            else if (len > 5 && len <= 8)
                return EncryptString(value, 2, 2, specialChar);
            else if (len > 8 && len <= 10)
                return EncryptString(value, 3, 3, specialChar);
            else
                return EncryptString(value, 4, 4, specialChar);
        }

        public static string EncryptString(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            int len = value.Length - startLen - endLen;

            if (len <= 0) return value;

            string left = value.Substring(0, startLen);
            string right = value.Substring(value.Length - endLen);

            return $"{left}{"".PadLeft(len, specialChar)}{right}";
        }
    }
}