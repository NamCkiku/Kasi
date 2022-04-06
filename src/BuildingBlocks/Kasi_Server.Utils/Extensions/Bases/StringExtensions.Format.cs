using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static string EncryptPlateNumberOfChina(string plateNumber, char specialChar = '*') => Format.EncryptPlateNumberOfChina(plateNumber, specialChar);

        public static string EncryptVinCode(string vinCode, char specialChar = '*') => Format.EncryptVinCode(vinCode, specialChar);

        public static string FormatMoney(this decimal money, bool isEncrypt = false) => Format.FormatMoney(money, isEncrypt);

        public static string EncryptPhone(this string value, char specialChar = '*') => Format.EncryptPhone(value, specialChar);

        public static string EncryptEmail(this string value, char specialChar = '*') => Format.EncryptEmail(value, specialChar);

        public static string EncryptSensitiveInfo(this string value, char specialChar = '*') => Format.EncryptSensitiveInfo(value, specialChar);

        public static string EncryptString(this string value, int startLen = 4, int endLen = 4, char specialChar = '*')
            => Format.EncryptString(value, startLen, endLen, specialChar);
    }
}