using Kasi_Server.Utils.Helpers;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key,
            TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out var obj) ? Conv.To<TValue>(obj) : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out var obj) ? obj : defaultValue;
        }

        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) && replaceExisted)
                {
                    dict[item.Key] = item.Value;
                    continue;
                }
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return dict;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TKey, TValue> setValue)
        {
            if (!dict.TryGetValue(key, out var value))
            {
                value = setValue(key);
                dict.Add(key, value);
            }
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            Func<TValue> addFunc)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            return dictionary[key] = addFunc();
        }

        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dic, Dictionary<TKey, TValue> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.ContainsKey(key))
                    dic[key] = newDic[key];
                else
                    dic.Add(key, newDic[key]);
            }

            return dic;
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            IComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
            return new SortedDictionary<TKey, TValue>(dictionary, comparer);
        }

        public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new SortedDictionary<TKey, TValue>(dictionary).OrderBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static string ToQueryString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in dictionary)
            {
                sb.Append($"{item.Key.ToString()}={item.Value.ToString()}&");
            }

            sb.TrimEnd("&");
            return sb.ToString();
        }

        public static string ToMapString<TKey, TValue>(this IDictionary<TKey, TValue> keyValuePairs, char keyValuePairDelimiter, char keyValueDelimeter)
        {
            var list = new List<string>();

            foreach (var item in keyValuePairs)
            {
                list.Add($"{item.Key}{keyValueDelimeter}{item.Value}");
            }

            return list.Join(keyValuePairDelimiter);
        }

        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (var item in dictionary.Where(x => x.Value.Equals(value)))
            {
                return item.Key;
            }

            return default(TKey);
        }

        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static Hashtable ToHashTable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var table = new Hashtable();
            foreach (var item in dictionary)
            {
                table.Add(item.Key, item.Value);
            }

            return table;
        }

        public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.ToDictionary(x => x.Value, x => x.Key);
        }

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static bool EqualsTo<TKey, TValue>(this IDictionary<TKey, TValue> sourceDict,
            IDictionary<TKey, TValue> targetDict)
        {
            if (sourceDict == null)
                throw new ArgumentNullException(nameof(sourceDict), $@"源字典对象不可为空！");
            if (targetDict == null)
                throw new ArgumentNullException(nameof(sourceDict), $@"目标字典对象不可为空！");
            if (sourceDict.Count != targetDict.Count)
                return false;
            if (!sourceDict.Any() && !targetDict.Any())
                return true;
            var sourceKeyValues = sourceDict.OrderBy(x => x.Key).ToArray();
            var targetKeyValues = targetDict.OrderBy(x => x.Key).ToArray();
            var sourceKeys = sourceKeyValues.Select(x => x.Key);
            var targetKeys = targetKeyValues.Select(x => x.Key);
            var sourceValues = sourceKeyValues.Select(x => x.Value);
            var targetValues = targetKeyValues.Select(x => x.Value);
            if (sourceKeys.EqualsTo(targetKeys) && sourceValues.EqualsTo(targetValues))
                return true;
            return false;
        }

        public static T ToModel<T>(this IDictionary<string, object> sourceDict) where T : class, new()
        {
            T t = new T();
            var modelProperties = t.GetType().GetProperties();

            foreach (var property in modelProperties)
            {
                try
                {
                    var dict = sourceDict.FirstOrDefault(t => t.Key.Equals(property.Name));
                    if (string.IsNullOrEmpty(dict.Value?.ToString()))
                        continue;
                    if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(t, System.Enum.Parse(property.PropertyType, dict.Value.ToString()));
                    }
                    else if (property.PropertyType.IsValueType)
                    {
                        switch (property.PropertyType.Name.ToLower())
                        {
                            case "guid":
                                property.SetValue(t, Guid.Parse(dict.Value?.ToString()));
                                break;

                            default:
                                property.SetValue(t, Convert.ChangeType(dict.Value, property.PropertyType));
                                break;
                        }
                    }
                    else
                    {
                        property.SetValue(t, dict.Value);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{DateTime.Now}, {t.GetType().Name} PropertyName={property.Name} PropertyType= {property.PropertyType.Name}, Type={property.GetType().DeclaringType} Convert Error, Exception:{ex.Message}");
                }
            }
            return t;
        }

        public static TModel ExChange<TKey, TValue, TModel>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue, TModel> call)
        {
            if (source == null || source.Count == 0)
                return default;

            if (source.ContainsKey(key))
                return call.Invoke(key, source[key]);

            return default;
        }

        public static IList<TModel> ExChange<TKey, TValue, TModel>(this IDictionary<TKey, TValue> source, IList<TKey> keys, Func<TKey, TValue, TModel> call)
        {
            var items = new List<TModel>();

            if (source == null || source.Count == 0)
                return null;

            if (keys == null || keys.Count == 0)
                return items;

            keys.ForEach(key =>
            {
                if (source.ContainsKey(key))
                    items.Add(call.Invoke(key, source[key]));
            });

            return items;
        }

        public static KeyValue<TKey, TValue> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source == null || source.Count == 0)
                return default;

            if (source.ContainsKey(key))
                return new KeyValue<TKey, TValue>(key, source[key]);

            return default;
        }

        public static IList<KeyValue<TKey, TValue>> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, string key, string split = ",")
        {
            var strKeys = key.Split(split, true);

            if (strKeys == null || strKeys.Length == 0)
                return new List<KeyValue<TKey, TValue>>();

            var keys = Array.ConvertAll(strKeys, input => Conv.To<TKey>(input));

            return ExChange<TKey, TValue>(source, keys);
        }

        public static IList<KeyValue<TKey, TValue>> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, IList<TKey> keys)
        {
            var items = new List<KeyValue<TKey, TValue>>();

            if (source == null || source.Count == 0)
                return null;

            if (keys == null || keys.Count == 0)
                return items;

            keys.ForEach(key =>
            {
                if (source.ContainsKey(key))
                    items.Add(new KeyValue<TKey, TValue>(key, source[key]));
            });

            return items;
        }
    }
}