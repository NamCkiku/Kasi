using System.Collections;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsEmpty(this Guid value) => value == Guid.Empty;

        public static bool IsEmpty(this Guid? value) => value == null || IsEmpty(value.Value);

        public static bool IsEmpty(this StringBuilder sb) => sb == null || sb.Length == 0 || sb.ToString().IsEmpty();

        public static bool IsEmpty<T>(this IEnumerable<T> list) => null == list || !list.Any();

        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) => null == dictionary || dictionary.Count == 0;

        public static bool IsEmpty(this IDictionary dictionary) => null == dictionary || dictionary.Count == 0;

        public static bool IsNull(this object target) => target.IsNull<object>();

        public static bool IsNull<T>(this T target) => ReferenceEquals(target, null);

        public static bool NotEmpty(this string value) => !string.IsNullOrWhiteSpace(value);

        public static bool NotEmpty(this Guid value) => value != Guid.Empty;

        public static bool NotEmpty(this Guid? value) => value != null && value != Guid.Empty;

        public static bool NotEmpty(this StringBuilder sb) => sb != null && sb.Length != 0 && sb.ToString().NotEmpty();

        public static bool NotEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return false;
            if (enumerable.Any())
                return true;
            return false;
        }

        public static bool IsZeroOrMinus(this short value) => value <= 0;

        public static bool IsZeroOrMinus(this int value) => value <= 0;

        public static bool IsZeroOrMinus(this long value) => value <= 0;

        public static bool IsZeroOrMinus(this float value) => value <= 0;

        public static bool IsZeroOrMinus(this double value) => value <= 0;

        public static bool IsZeroOrMinus(this decimal value) => value <= 0;

        public static bool IsPercentage(this float value) => value > 0 && value <= 1;

        public static bool IsPercentage(this double value) => value > 0 && value <= 1;

        public static bool IsPercentage(this decimal value) => value > 0 && value <= 1;

        public static bool IsZeroOrPercentage(this float value) => value.IsPercentage() || value.Equals(0f);

        public static bool IsZeroOrPercentage(this double value) => value.IsPercentage() || value.Equals(0d);

        public static bool IsZeroOrPercentage(this decimal value) => value.IsPercentage() || value.Equals(0m);
    }
}