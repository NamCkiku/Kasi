namespace Kasi_Server.Utils.Helpers
{
    public static class Common
    {
        public static Type GetType<T>() => GetType(typeof(T));

        public static Type GetType(Type type) => Nullable.GetUnderlyingType(type) ?? type;

        public static string Line => Environment.NewLine;

        public static void Swap<T>(ref T a, ref T b)
        {
            var swap = a;
            a = b;
            b = swap;
        }
    }
}