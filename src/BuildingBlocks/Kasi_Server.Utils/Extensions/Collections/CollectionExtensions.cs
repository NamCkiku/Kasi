using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            Check.NotNull(source, nameof(source));

            if (source.Contains(item))
            {
                return false;
            }
            source.Add(item);
            return true;
        }

        public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            Check.NotNull(source, nameof(source));

            var addedItems = new List<T>();

            foreach (var item in items)
            {
                if (source.Contains(item))
                {
                    continue;
                }

                source.Add(item);
                addedItems.Add(item);
            }

            return addedItems;
        }

        public static bool AddIfNotContains<T>(this ICollection<T> source, Func<T, bool> predicate, Func<T> itemFactory)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(itemFactory, nameof(itemFactory));

            if (source.Any(predicate))
            {
                return false;
            }

            source.Add(itemFactory());
            return true;
        }

        public static void RemoveAll<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(items, nameof(items));

            foreach (var item in items)
            {
                source.Remove(item);
            }
        }

        public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));

            var items = source.Where(predicate).ToList();
            foreach (var item in items)
            {
                source.Remove(item);
            }

            return items;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), $@"源{typeof(T).Name}集合对象不可为空！");
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), $@"要添加的{typeof(T).Name}集合项不可为空！");
            enumerable.ForEach(collection.Add);
        }

        public static void Sort<T>(this ICollection<T> collection, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            var list = new List<T>(collection);
            list.Sort(comparer);
            collection.ReplaceItems(list);
        }

        public static void ReplaceItems<TItem, TNewItem>(this ICollection<TItem> collection,
            IEnumerable<TNewItem> newItems, Func<TNewItem, TItem> createItemAction)
        {
            collection.CheckNotNull(nameof(collection));
            newItems.CheckNotNull(nameof(newItems));
            createItemAction.CheckNotNull(nameof(createItemAction));

            collection.Clear();
            var convertedNewItems = newItems.Select(createItemAction);
            collection.AddRange(convertedNewItems);
        }

        public static void ReplaceItems<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            collection.CheckNotNull(nameof(collection));
            newItems.CheckNotNull(nameof(newItems));

            collection.ReplaceItems(newItems, x => x);
        }
    }
}