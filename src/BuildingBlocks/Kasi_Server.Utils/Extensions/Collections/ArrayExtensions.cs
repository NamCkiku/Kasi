namespace Kasi_Server.Utils.Extensions
{
    public static partial class ArrayExtensions
    {
        public static bool WithInIndex(this Array source, int index)
        {
            return source != null && index >= 0 && index < source.Length;
        }

        public static bool WithInIndex(this Array source, int index, int dimension)
        {
            return source != null && index >= source.GetLowerBound(dimension) &&
                   index <= source.GetUpperBound(dimension);
        }

        public static T[] CombineArray<T>(this T[] combineWith, T[] arrayToCombine)
        {
            if (combineWith != default(T[]) && arrayToCombine != default(T[]))
            {
                int initialSize = combineWith.Length;
                Array.Resize(ref combineWith, initialSize + arrayToCombine.Length);
                Array.Copy(arrayToCombine, arrayToCombine.GetLowerBound(0), combineWith, initialSize,
                    arrayToCombine.Length);
            }
            return combineWith;
        }

        public static Array ClearAll(this Array source)
        {
            if (source != null)
            {
                Array.Clear(source, 0, source.Length);
            }

            return source;
        }

        public static T[] ClearAll<T>(this T[] source)
        {
            if (source != null)
            {
                for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); ++i)
                {
                    source[i] = default(T);
                }
            }

            return source;
        }

        public static Array ClearAt(this Array array, int index)
        {
            if (array != null)
            {
                var arrayIndex = index.GetArrayIndex();
                if (arrayIndex.IsIndexInArray(array))
                {
                    Array.Clear(array, arrayIndex, 1);
                }
            }

            return array;
        }

        public static T[] ClearAt<T>(this T[] array, int index)
        {
            if (array != null)
            {
                var arrayIndex = index.GetArrayIndex();
                if (arrayIndex.IsIndexInArray(array))
                {
                    array[arrayIndex] = default(T);
                }
            }

            return array;
        }

        public static T[] BlockCopy<T>(this T[] source, int index, int length)
        {
            return BlockCopy(source, index, length, false);
        }

        public static T[] BlockCopy<T>(this T[] source, int index, int length, bool padToLength)
        {
            if (source == null)
            {
                throw new NullReferenceException(nameof(source));
            }

            int n = length;
            T[] b = null;
            if (source.Length < index + length)
            {
                n = source.Length - index;
                if (padToLength)
                {
                    b = new T[length];
                }
            }

            if (b == null)
            {
                b = new T[n];
            }
            Array.Copy(source, index, b, 0, n);
            return b;
        }

        public static IEnumerable<T[]> BlockCopy<T>(this T[] source, int length, bool padToLength)
        {
            for (int i = 0; i < source.Length; i += length)
            {
                yield return source.BlockCopy(i, length, padToLength);
            }
        }
    }
}