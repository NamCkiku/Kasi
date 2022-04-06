using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class ListExtensions
    {
        public static bool InsertIfNotExists<T>(this IList<T> list, int index, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Insert(index, item);
                return true;
            }

            return false;
        }

        public static int InsertIfNotExists<T>(this IList<T> list, int startIndex, IEnumerable<T> items)
        {
            var index = startIndex + items.Reverse().Count(item => list.InsertIfNotExists(startIndex, item));
            return (index - startIndex);
        }

        public static int IndexOf<T>(this IList<T> list, Func<T, bool> comparison)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (comparison(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static string Join<T>(this IList<T> list, char joinChar)
        {
            return list.Join(joinChar.ToString());
        }

        public static string Join<T>(this IList<T> list, string joinString)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int listCount = list.Count;
            int listCountMinusOne = listCount - 1;

            if (listCount > 1)
            {
                for (var i = 0; i < listCount; i++)
                {
                    if (i != listCountMinusOne)
                    {
                        sb.Append(list[i]);
                        sb.Append(joinString);
                    }
                    else
                    {
                        sb.Append(list[i]);
                    }
                }
            }
            else
            {
                sb.Append(list[0]);
            }

            return sb.ToString();
        }

        public static bool EqualsAll<T>(this IList<T> list, IList<T> other)
        {
            if (list == null || other == null)
            {
                return list == null && other == null;
            }

            if (list.Count != other.Count)
            {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < list.Count; i++)
            {
                if (!comparer.Equals(list[i], other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static IList<T> Slice<T>(this IList<T> list, int? start, int? end)
        {
            return Slice(list, start, end, null);
        }

        public static IList<T> Slice<T>(this IList<T> list, int? start, int? end, int? step)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (step == 0)
            {
                throw new ArgumentException($"{nameof(step)} 不能为0");
            }

            List<T> result = new List<T>();

            if (list.Count == 0)
            {
                return result;
            }

            var s = step ?? 1;
            var startIndex = start ?? 0;
            var endIndex = end ?? list.Count;

            startIndex = (startIndex < 0) ? list.Count + startIndex : startIndex;
            endIndex = (endIndex < 0) ? list.Count + endIndex : endIndex;

            startIndex = Math.Max(startIndex, 0);
            endIndex = Math.Min(endIndex, list.Count - 1);

            for (int i = startIndex; i < endIndex; i += s)
            {
                result.Add(list[i]);
            }

            return result;
        }

        public static List<T> ToTree<T>(this IList<T> list, Func<T, T, bool> rootWhere, Func<T, T, bool> childsWhere, Action<T, IEnumerable<T>> addChilds, T entity = default)
        {
            var treelist = new List<T>();
            if (list == null || list.Count == 0)
            {
                return treelist;
            }
            if (!list.Any(e => rootWhere(entity, e)))
            {
                return treelist;
            }

            if (list.Any(e => rootWhere(entity, e)))
            {
                treelist.AddRange(list.Where(e => rootWhere(entity, e)));
            }

            foreach (var item in treelist)
            {
                if (list.Any(e => childsWhere(item, e)))
                {
                    var nodedata = list.Where(e => childsWhere(item, e)).ToList();
                    foreach (var child in nodedata)
                    {
                        var data = list.ToTree(childsWhere, childsWhere, addChilds, child);
                        addChilds(child, data);
                    }
                    addChilds(item, nodedata);
                }
            }

            return treelist;
        }
    }
}