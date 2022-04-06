using Kasi_Server.Utils.Helpers;
using System.Collections.Specialized;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection collection)
        {
            if (collection == null || !collection.HasKeys())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string key in collection.Keys)
            {
                sb.Append($"{key}={collection[key]}&");
            }

            sb.TrimEnd("&");
            return sb.ToString();
        }

        public static string GetOrDefault(NameValueCollection collection, string key, string defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }

        public static T GetOrDefault<T>(NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return Conv.To<T>(val);
        }
    }
}