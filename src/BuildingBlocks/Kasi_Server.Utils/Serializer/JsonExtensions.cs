using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kasi_Server.Utils.Serializer
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this string json, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObject<T>(json, options);
        }

        public static object ToObject(this string json, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObject(json, options);
        }

        public static string ToJson(this object target)
        {
            return JsonHelper.ToJson(target);
        }

        public static string ToJson(this object target, bool isConvertToSingleQuotes = false, bool camelCase = false,
            bool indented = false)
        {
            return JsonHelper.ToJson(target, isConvertToSingleQuotes, camelCase, indented);
        }

        public static string ToJson(this object target, JsonSerializerSettings options)
        {
            return JsonHelper.ToJson(target, options);
        }

        public static JObject ToJObject(this string json)
        {
            return JsonHelper.ToJObject(json);
        }

        public static bool IsJson(this string json)
        {
            return JsonHelper.IsJson(json);
        }

        public static IList<T> ToObjectNotNullOrEmpty<T>(this IList<string> list, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObjectNotNullOrEmpty<T>(list, options);
        }

        public static string ToJsonNotNullOrEmpty(this IList<string> list)
        {
            return JsonHelper.ToJsonNotNullOrEmpty(list);
        }

        public static IList<T> ToObject<T>(this IList<string> list, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObject<T>(list, options);
        }

        public static string ToJson(this IList<string> list)
        {
            return JsonHelper.ToJson(list);
        }
    }
}