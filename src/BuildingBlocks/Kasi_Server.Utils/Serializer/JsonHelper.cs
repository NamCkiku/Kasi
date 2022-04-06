using Kasi_Server.Utils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Serializer
{
    public static class JsonHelper
    {
        public static string JsonDateTimeFormat(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return json;
            }

            json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            });
            return json;
        }

        public static T ToObject<T>(string json, JsonSerializerSettings options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json, options);
        }

        public static object ToObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static object ToObject(string json, JsonSerializerSettings options = null)
        {
            return JsonConvert.DeserializeObject(json, options);
        }

        public static string ToJson(object target)
        {
            if (target == null)
            {
                return string.Empty;
            }

            var result = JsonConvert.SerializeObject(target);

            return result;
        }

        public static string ToJson(object target, bool isConvertToSingleQuotes = false, bool camelCase = false,
            bool indented = false)
        {
            var options = new JsonSerializerSettings();
            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }

            var result = ToJson(target, options);

            if (isConvertToSingleQuotes)
            {
                result = result.Replace("\"", "'");
            }

            return result;
        }

        public static string ToJson(object target, JsonSerializerSettings options)
        {
            if (target == null)
            {
                return string.Empty;
            }

            var result = JsonConvert.SerializeObject(target, options);

            return result;
        }

        public static void SerializableToFile(string fileName, object obj)
        {
            lock (obj)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(ToJson(obj, false, false, true));
                    }
                }
            }
        }

        public static T DeserializeFromFile<T>(string fileName, JsonSerializerSettings options = null)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        return ToObject<T>(sr.ReadToEnd(), options);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToJsonByForm(string formStr)
        {
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            var data = formStr.Split('&');
            for (int i = 0; i < data.Length; i++)
            {
                var dk = data[i].Split('=');
                StringBuilder sb = new StringBuilder(dk[1]);
                for (int j = 2; j <= dk.Length - 1; j++)
                {
                    sb.Append(dk[j]);
                }

                dicData.Add(dk[0], sb.ToString());
            }

            return dicData.ToJson();
        }

        public static JObject ToJObject(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return JObject.Parse("{}");
            }

            return JObject.Parse(json.Replace("&nbsp;", ""));
        }

        public static bool IsJson(string json)
        {
            json = json.Trim();
            return json.StartsWith("{") && json.EndsWith("}") || json.StartsWith("[") && json.EndsWith("]");
        }

        public static IList<T> ToObjectNotNullOrEmpty<T>(IList<string> list, JsonSerializerSettings options = null)
        {
            return ToJsonNotNullOrEmpty(list).ToObject<List<T>>(options);
        }

        public static string ToJsonNotNullOrEmpty(IList<string> list)
        {
            if (list == null || list.Count() == 0)
                return string.Empty;

            return $"[{list.Where(t => !string.IsNullOrEmpty(t)).ToArray().Join(",")}]";
        }

        public static IList<T> ToObject<T>(IList<string> list, JsonSerializerSettings options = null)
        {
            return ToJson(list).ToObject<List<T>>(options);
        }

        public static string ToJson(IList<string> list)
        {
            if (list == null || list.Count() == 0)
                return string.Empty;

            return $"[{list.ToArray().Join(",")}]";
        }
    }
}