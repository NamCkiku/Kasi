namespace Kasi_Server.Utils.Paging
{
    using System;
    using System.Text;

    public class PagerBuilderWeb : IPagerBuilderWeb
    {
        private static IPagerBuilderWeb _instance;
        private static readonly object _syncRoot = new object();

        public static IPagerBuilderWeb Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PagerBuilderWeb();
                        }
                    }
                }
                return _instance;
            }
        }

        public string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, PagerSettings.Default);
            return Build(pager, PagerSettings.Default, urlBuilder);
        }

        public string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, settings);
            return Build(pager, pager.Settings, urlBuilder);
        }

        public string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pagerData = pager;

            StringBuilder buffer = new StringBuilder();
            string cssClass = string.Empty;
            string urlParams = string.Empty;
            string url = string.Empty;

            string cssClassForPage = string.IsNullOrEmpty(settings.CssClass) ? string.Empty : " class=\"" + settings.CssClass + "\"";

            if (pagerData.CanShowFirst && settings.ShowFirstAndLastPage)
            {
                url = urlBuilder(1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.First + "</a></li>");

                if (pagerData.CanShowPrevious)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
            }

            if (pagerData.CurrentPage > 1)
            {
                url = urlBuilder(pagerData.CurrentPage - 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Prev + "</a></li>");
            }
            if (pagerData.CanShowPrevious)
            {
                url = urlBuilder(pagerData.StartingPage - 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + "..." + "</a></li>");
            }
            for (int ndx = pagerData.StartingPage; ndx <= pagerData.EndingPage; ndx++)
            {
                cssClass = (ndx == pagerData.CurrentPage) ? " class=\"" + settings.CssCurrentPage + "\"" : cssClassForPage;
                url = urlBuilder(ndx);

                url = (ndx == pagerData.CurrentPage) ? string.Empty : " href=\"" + url + "\"";

                buffer.Append("<li" + cssClass + "><a" + url + ">" + ndx + "</a></li>");
            }

            if (pagerData.CanShowNext)
            {
                url = urlBuilder(pagerData.EndingPage + 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + "..." + "</a></li>");
            }
            if (pagerData.CurrentPage < pagerData.TotalPages)
            {
                url = urlBuilder(pagerData.CurrentPage + 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Next + "</a></li>");
            }
            if (pagerData.CanShowLast && settings.ShowFirstAndLastPage)
            {
                url = urlBuilder(pagerData.TotalPages);

                if (pagerData.CanShowNext)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Last + "</a></li>");
            }
            string content = buffer.ToString();
            return content;
        }
    }
}