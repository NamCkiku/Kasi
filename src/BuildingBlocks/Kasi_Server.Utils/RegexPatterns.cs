namespace Kasi_Server.Utils
{
    public class RegexPatterns
    {
        public const string Number = @"^[0-9]+$";

        public const string NumberSign = @"^[+-]?[0-9]+$";

        public const string ZHCN = @"[\u4e00-\u9fa5]";

        public const string Alpha = @"^[a-zA-Z]*$";

        public const string AlphaUpperCase = @"^[A-Z]*$";

        public const string AlphaLowerCase = @"^[a-z]*$";

        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";

        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";

        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";

        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";

        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";

        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";

        public const string ChinesePattern = @"^[\u4e00-\u9fa5]+$";

        public const string IdentityCard = @"(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}(?:\d|x|X)$)";

        public const string Email = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";

        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";

        public const string ZipCode = @"^[1-9]\d{5}$";

        public const string MobilePhone = @"^1[0-9]{10}";

        public const string TelPhone = @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$";

        public const string WechatNoPatter = @"^[a-zA-Z]([-_a-zA-Z0-9]{5,19})+$";

        public const string TelNoOfChinaPatter = @"^\d{3,4}-?\d{6,8}$";

        public const string PlateNumberOfChinaPatter =
            @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$";

        public const string PostalCodeOfChinaPatter = @"^\d{6}$";

        public const string QQPatter = @"^[1-9][0-9]{4,10}$";
    }
}