namespace Kasi_Server.Utils.Paging
{
    public class PagerLanguage
    {
        public static readonly PagerLanguage Default = new PagerLanguage("First", "&#171;", "&#187;", "Last");

        public PagerLanguage(string first, string prev, string next, string last)
        {
            First = first;
            Prev = prev;
            Next = next;
            Last = last;
        }

        public string First = "First";
        public string Prev = "Prev";
        public string Next = "Next";
        public string Last = "Last";
    }

    public class PagerSettings
    {
        public static readonly PagerSettings Default = new PagerSettings(7, "current", "", false, PagerLanguage.Default);

        public PagerSettings()
        { }

        public PagerSettings(int numberPagesToDisplay, string cssClassForCurrentPage,
            string cssClassForPage, bool showFirstAndLastPage, PagerLanguage language)
        {
            NumberPagesToDisplay = numberPagesToDisplay;
            CssCurrentPage = cssClassForCurrentPage;
            CssClass = cssClassForPage;
            ShowFirstAndLastPage = showFirstAndLastPage;
            Language = language;
        }

        public int NumberPagesToDisplay = 5;

        public string CssCurrentPage = string.Empty;

        public string CssClass = string.Empty;

        public bool ShowFirstAndLastPage = false;

        public PagerLanguage Language = PagerLanguage.Default;
    }
}