namespace Kasi_Server.Utils.Extensions
{
    public static partial class StringExtensions
    {
        public static bool IsImageFile(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }

            byte[] filedata = File.ReadAllBytes(fileName);
            if (filedata.Length == 0)
            {
                return false;
            }
            ushort code = BitConverter.ToUInt16(filedata, 0);
            switch (code)
            {
                case 0x4D42:
                case 0xD8FF:
                case 0x4947:
                case 0x5089:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsLikeAny(this string value, params string[] patterns)
        {
            return patterns.Any(value.IsLike);
        }

        public static bool IsLike(this string value, string pattern)
        {
            if (value == pattern)
            {
                return true;
            }
            if (pattern[0] == '*' && pattern.Length > 1)
            {
                return value.Where((t, index) => value.Substring(index).IsLike(pattern.Substring(1))).Any();
            }

            if (pattern[0] == '*')
            {
                return true;
            }

            if (pattern[0] == value[0])
            {
                return value.Substring(1).IsLike(pattern.Substring(1));
            }
            return false;
        }

        public static Func<bool> IsItemInEnum<TEnum>(this string value) where TEnum : struct
        {
            return () => string.IsNullOrEmpty(value) || !Enum.IsDefined(typeof(TEnum), value);
        }

        public static bool IsRangeLength(this string source, int minLength, int maxLength)
        {
            return source.Length >= minLength && source.Length <= maxLength;
        }

        public static bool EqualsAny(this string value, StringComparison comparisonType, params string[] values)
        {
            return values.Any(v => value.Equals(v, comparisonType));
        }

        public static bool EquivalentTo(this string value, string whateverCaseString, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            return string.Equals(value, whateverCaseString, comparison);
        }

        public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
        {
            return (inputValue.IndexOf(comparisonValue, comparisonType) != -1);
        }

        public static bool ContainsEquivalenceTo(this string inputValue, string comparisonValue)
        {
            return BothStringsAreEmpty(inputValue, comparisonValue) ||
                   StringContainsEquivalence(inputValue, comparisonValue);
        }

        private static bool BothStringsAreEmpty(string inputValue, string comparisonValue)
        {
            return (inputValue.IsEmpty() && comparisonValue.IsEmpty());
        }

        private static bool StringContainsEquivalence(string inputValue, string comparisonValue)
        {
            return ((!inputValue.IsEmpty()) && inputValue.Contains(comparisonValue, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool ContainsAny(this string value, params string[] values)
        {
            return value.ContainsAny(StringComparison.CurrentCulture, values);
        }

        public static bool ContainsAny(this string value, StringComparison comparisonType, params string[] values)
        {
            return values.Any(v => value.IndexOf(v, comparisonType) > -1);
        }

        public static bool ContainsAll(this string value, params string[] values)
        {
            return value.ContainsAll(StringComparison.CurrentCulture, values);
        }

        public static bool ContainsAll(this string value, StringComparison comparisonType, params string[] values)
        {
            return values.All(v => value.IndexOf(v, comparisonType) > -1);
        }
    }
}