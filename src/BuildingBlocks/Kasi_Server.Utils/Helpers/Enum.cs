using Kasi_Server.Utils.Extensions;
using System.ComponentModel;
using System.Reflection;

namespace Kasi_Server.Utils.Helpers
{
    public static class Enum
    {
        private const string EnumValueField = "value__";

        public static TEnum Parse<TEnum>(object member)
        {
            var value = member.SafeString();
            if (value.IsEmpty())
            {
                if (typeof(TEnum).IsGenericType)
                    return default;
                throw new ArgumentNullException(nameof(member));
            }
            return (TEnum)System.Enum.Parse(Common.GetType<TEnum>(), value, true);
        }

        public static TEnum ParseByDescription<TEnum>(string desc)
        {
            if (desc.IsEmpty())
            {
                if (typeof(TEnum).IsGenericType)
                    return default;
                throw new ArgumentNullException(nameof(desc));
            }
            var type = Common.GetType<TEnum>();
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Default);
            var fieldInfo =
                fieldInfos.FirstOrDefault(p => p.GetCustomAttribute<DescriptionAttribute>(false)?.Description == desc);
            if (fieldInfo == null)
                throw new ArgumentNullException($"在枚举（{type.FullName}）中，未发现描述为“{desc}”的枚举项。");
            return (TEnum)System.Enum.Parse(type, fieldInfo.Name);
        }

        public static string GetName<TEnum>(object member) => GetName(Common.GetType<TEnum>(), member);

        public static string GetName(Type type, object member)
        {
            if (type == null)
                return string.Empty;
            if (member == null)
                return string.Empty;
            if (member is string)
                return member.ToString();
            if (type.GetTypeInfo().IsEnum == false)
                return string.Empty;
            return System.Enum.GetName(type, member);
        }

        public static string[] GetNames<TEnum>() where TEnum : struct => GetNames(typeof(TEnum));

        public static string[] GetNames(Type type) => System.Enum.GetNames(type);

        public static int GetValue<TEnum>(object member) => GetValue(Common.GetType<TEnum>(), member);

        public static int GetValue(Type type, object member)
        {
            string value = member.SafeString();
            if (value.IsEmpty())
                throw new ArgumentNullException(nameof(member));
            return (int)System.Enum.Parse(type, member.ToString(), true);
        }

        public static string GetDescription<TEnum>(object member) => Reflection.GetDescription<TEnum>(GetName<TEnum>(member));

        public static string GetDescription(Type type, object member) => Reflection.GetDescription(type, GetName(type, member));

        public static List<Item> GetItems<TEnum>() => GetItems(typeof(TEnum));

        public static List<Item> GetItems(Type type)
        {
            type = Common.GetType(type);
            ValidateEnum(type);
            var result = new List<Item>();
            foreach (var field in type.GetFields())
                AddItem(type, result, field);
            return result.OrderBy(t => t.SortId).ToList();
        }

        private static void ValidateEnum(Type enumType)
        {
            if (enumType.IsEnum == false)
                throw new InvalidOperationException($"类型 {enumType} 不是枚举");
        }

        private static void AddItem(Type type, ICollection<Item> result, FieldInfo field)
        {
            if (!field.FieldType.IsEnum)
                return;
            var value = GetValue(type, field.Name);
            var description = Reflection.GetDescription(field);
            result.Add(new Item(description, value, value));
        }

        public static IDictionary<int, string> GetDictionary<TEnum>(params int[] excludeKey) where TEnum : struct
        {
            var dic = GetDictionary<TEnum>();

            if (excludeKey != null && excludeKey.Length > 0)
                excludeKey.ForEach(key => dic.Remove(key));

            return dic;
        }

        public static IDictionary<int, string> GetDictionary<TEnum>() where TEnum : struct
        {
            var enumType = Common.GetType<TEnum>().GetTypeInfo();
            ValidateEnum(enumType);
            var dic = new Dictionary<int, string>();
            foreach (var field in enumType.GetFields())
                AddItem<TEnum>(dic, field);
            return dic;
        }

        private static void AddItem<TEnum>(IDictionary<int, string> result, FieldInfo field) where TEnum : struct
        {
            if (!field.FieldType.GetTypeInfo().IsEnum)
                return;
            var value = GetValue<TEnum>(field.Name);
            var description = Reflection.GetDescription(field);
            result.Add(value, description);
        }

        public static IEnumerable<Tuple<int, string, string>> GetMemberInfos<TEnum>() where TEnum : struct
        {
            var type = typeof(TEnum);
            ValidateEnum(type);
            var fields = type.GetFields();
            ICollection<Tuple<int, string, string>> collection = new HashSet<Tuple<int, string, string>>();
            foreach (var field in fields.Where(x => x.Name != EnumValueField))
            {
                var value = GetValue<TEnum>(field.Name);
                var description = Reflection.GetDescription(field);
                collection.Add(new Tuple<int, string, string>(value, field.Name,
                    string.IsNullOrWhiteSpace(description) ? field.Name : description));
            }

            return collection;
        }
    }
}