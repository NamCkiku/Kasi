namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static void Locking(this object source, Action action)
        {
            lock (source)
                action();
        }

        public static void Locking<T>(this T source, Action<T> action)
        {
            lock (source)
                action(source);
        }

        public static TResult Locking<TResult>(this object source, Func<TResult> func)
        {
            lock (source)
                return func();
        }

        public static TResult Locking<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
        {
            lock (source)
                return func(source);
        }
    }
}