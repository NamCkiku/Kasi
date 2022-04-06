namespace Kasi_Server.Utils.Conversions.Internals
{
    internal static class Helper
    {
        public static bool IsXXX<TFrom, TTo>(
            TFrom @from,
            Func<TFrom, bool> fromTry,
            Func<TFrom, Action<TTo>, bool> firstTry,
            IEnumerable<IConversionTry<TFrom, TTo>> tries,
            Action<TTo> act = null)
        {
            if (fromTry(from))
                return false;
            if (firstTry(from, act))
                return true;
            if (tries is null)
                return false;
            foreach (var @try in tries)
            {
                if (@try.Is(from, out var to))
                {
                    act?.Invoke(to);
                    return true;
                }
            }
            return false;
        }

        public static TTo ToXXX<TFrom, TTo>(
            TFrom @from,
            Func<TFrom, Action<TTo>, bool> firstImpl,
            IEnumerable<IConversionImpl<TFrom, TTo>> impls)
        {
            TTo result = default;
            if (firstImpl(from, to => result = to))
                return result;
            if (impls is null)
                return result;
            foreach (var impl in impls)
            {
                if (impl.TryTo(from, out result))
                    return result;
            }
            return result;
        }
    }
}