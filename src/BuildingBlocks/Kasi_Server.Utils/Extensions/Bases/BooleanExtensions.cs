namespace Kasi_Server.Utils.Extensions
{
    public static class BooleanExtensions
    {
        public static string ToLower(this bool value)
        {
            return value.ToString().ToLower();
        }

        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }

        public static int ToBinaryTypeNumber(this bool value)
        {
            return value ? 1 : 0;
        }

        public static string ToChineseString(this bool value)
        {
            return ToChineseString(value, "是", "否");
        }

        public static string ToChineseString(this bool value, string trueStr, string falseStr)
        {
            return value ? trueStr : falseStr;
        }

        public static string ToChineseString(this bool? value)
        {
            return ToChineseString(value, "是", "否");
        }

        public static string ToChineseString(this bool? value, string trueStr, string falseStr)
        {
            return value.GetValueOrDefault() ? trueStr : falseStr;
        }

        public static T IfTrue<T>(this bool value, T t)
        {
            return value ? t : default(T);
        }

        public static T IfTrue<T>(this bool? value, T t)
        {
            return value.GetValueOrDefault() ? t : default(T);
        }

        public static void IfTrue(this bool value, Action action)
        {
            if (value)
            {
                action();
            }
        }

        public static void IfTrue(this bool? value, Action action)
        {
            if (value.GetValueOrDefault())
            {
                action();
            }
        }

        public static T IfFalse<T>(this bool value, T t)
        {
            return !value ? t : default(T);
        }

        public static T IfFalse<T>(this bool? value, T t)
        {
            return !value.GetValueOrDefault() ? t : default(T);
        }

        public static void IfFalse(this bool value, Action action)
        {
            if (!value)
            {
                action();
            }
        }

        public static void IfFalse(this bool? value, Action action)
        {
            if (!value.GetValueOrDefault())
            {
                action();
            }
        }

    }
}