namespace Kasi_Server.Utils.Helpers
{
    public static class Url
    {
        #region Combine(合并Url)

        public static string Combine(params string[] urls)
        {
            return Path.Combine(urls).Replace(@"\", "/");
        }

        #endregion Combine(合并Url)

        #region Join(连接Url)

        public static string Join(string url, string param)
        {
            return $"{GetUrl(url)}{param}";
        }

        public static string Join(string url, params string[] parameters)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (parameters.Length == 0)
            {
                return url;
            }
            var currentUrl = Join(url, parameters[0]);
            return Join(currentUrl, parameters.Skip(1).ToArray());
        }

        private static string GetUrl(string url)
        {
            if (!url.Contains("?"))
            {
                return $"{url}?";
            }

            if (url.EndsWith("?"))
            {
                return url;
            }

            if (url.EndsWith("&"))
            {
                return url;
            }

            return $"{url}&";
        }

        public static Uri Join(Uri url, string param)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            return new Uri(Join(url.AbsoluteUri, param));
        }

        public static Uri Join(Uri url, params string[] parameters)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            return new Uri(Join(url.AbsoluteUri, parameters));
        }

        #endregion Join(连接Url)

        #region GetMainDomain(获取主域名)

        public static string GetMainDomain(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }
            var array = url.Split('.');

            if (array.Length != 3)
            {
                return url;
            }

            var tok = new List<string>(array);
            var remove = array.Length - 2;
            tok.RemoveRange(0, remove);
            return tok[0] + "." + tok[1];
        }

        #endregion GetMainDomain(获取主域名)
    }
}