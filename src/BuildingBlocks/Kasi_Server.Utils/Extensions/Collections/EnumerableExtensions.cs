using Kasi_Server.Utils.Serializer;
using System.Data;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static IList<TResult> ToMap<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");
            return enumerable.ForEach(converter);
        }

        public static IList<TResult> ToMap<T, TResult>(this IEnumerable<T> enumerable, Func<T, int, TResult> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");
            return enumerable.ForEach(converter);
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<T> enumerable, IEnumerable<T> collection)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");

            if (collection == null)
                return enumerable;

            var list = enumerable.ToList();

            list.AddRange(collection);

            return list;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (action == null)
                throw new ArgumentNullException(nameof(action), @"操作表达式不可为空！");
            foreach (var item in enumerable)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (action == null)
                throw new ArgumentNullException(nameof(action), @"操作表达式不可为空！");

            int i = 0;
            foreach (var item in enumerable)
                action(item, i++);
        }

        public static string ForEach<T>(this IEnumerable<T> enumerable, Func<T, string> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");

            string val = string.Empty;

            enumerable.ForEach<T>(item => { val += converter(item); });

            return val;
        }

        public static IList<TResult> ForEach<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");

            var list = new List<TResult>();

            enumerable.ForEach<T>(item => { list.Add(converter(item)); });

            return list;
        }

        public static string ForEach<T>(this IEnumerable<T> enumerable, Func<T, int, string> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");

            string val = string.Empty;

            enumerable.ForEach<T>((item, ndx) => { val += converter(item, ndx); });

            return val;
        }

        public static IList<TResult> ForEach<T, TResult>(this IEnumerable<T> enumerable, Func<T, int, TResult> converter)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (converter == null)
                throw new ArgumentNullException(nameof(converter), @"操作表达式不可为空！");

            var list = new List<TResult>();

            enumerable.ForEach<T>((item, ndx) => { list.Add(converter(item, ndx)); });

            return list;
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (action == null)
                throw new ArgumentNullException(nameof(action), @"操作表达式不可为空！");
            return Task.WhenAll(from item in enumerable select Task.Run(() => action(item)));
        }

        public static bool EqualsTo<T>(this IEnumerable<T> sourceList, IEnumerable<T> targetList)
        {
            if (sourceList == null)
                throw new ArgumentNullException(nameof(sourceList), $@"源{typeof(T).Name}集合对象不可为空！");
            if (targetList == null)
                throw new ArgumentNullException(nameof(targetList), $@"目标{typeof(T).Name}集合对象不可为空！");
            if (sourceList.Count() != targetList.Count())
                return false;
            if (!sourceList.Any() && !targetList.Any())
                return true;
            if (!sourceList.Except(targetList).Any() && !targetList.Except(sourceList).Any())
                return true;
            var sourceListStr = sourceList.ToJson().Trim();
            var targetListStr = targetList.ToJson().Trim();
            if (sourceListStr == targetListStr)
                return true;
            return false;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector), @"参照字段表达式不可为空");
            enumerable = enumerable as IList<T> ?? enumerable.ToList();
            var seenKeys = new HashSet<TKey>();
            return enumerable.Where(item => seenKeys.Add(keySelector(item)));
        }

        public static string ExpandAndToString<T>(this IEnumerable<T> collection, string separator = ",",
            string wrapItem = "") =>
            collection.ExpandAndToString(t => t.ToString(), separator, wrapItem);

        public static string ExpandAndToString<T>(this IEnumerable<T> collection, Func<T, string> itemFormatFunc,
            string separator = ",", string wrapItem = "")
        {
            collection = collection as IList<T> ?? collection.ToList();
            itemFormatFunc.CheckNotNull(nameof(itemFormatFunc));
            if (!collection.Any())
                return null;
            var sb = new StringBuilder();
            var i = 0;
            var count = collection.Count();
            foreach (var t in collection)
            {
                if (!string.IsNullOrWhiteSpace(wrapItem))
                {
                    sb.Append(i == count - 1
                        ? $"{wrapItem}{itemFormatFunc(t)}{wrapItem}"
                        : $"{wrapItem}{itemFormatFunc(t)}{wrapItem}{separator}");
                }
                else
                {
                    if (i == count - 1)
                        sb.Append(itemFormatFunc(t));
                    else
                        sb.Append(itemFormatFunc(t) + separator);
                }
                i++;
            }
            return sb.ToString();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable, string tableName = "")
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            var type = typeof(T);
            var properties = type.GetProperties();
            var dataTable = string.IsNullOrEmpty(tableName) ? new DataTable() : new DataTable(tableName);
            foreach (var property in properties)
            {
                dataTable.Columns.Add(new DataColumn(property.Name));
            }

            var array = enumerable.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                foreach (var property in properties)
                {
                    dataTable.Rows[i][property.Name] = property.GetValue(array[i]);
                }
            }

            return dataTable;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, bool condition)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"源{typeof(T).Name}集合对象不可为空！");
            enumerable = enumerable as IList<T> ?? enumerable.ToList();
            return condition ? enumerable.Where(predicate) : enumerable;
        }
    }
}