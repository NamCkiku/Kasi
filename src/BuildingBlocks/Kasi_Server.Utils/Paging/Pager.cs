namespace Kasi_Server.Utils.Paging
{
    using System;

    public partial class Pager : ICloneable
    {
        private int _currentPage;
        private int _totalPages;
        private int _previousPage;
        private int _startingPage;
        private int _endingPage;
        private int _nextPage;
        private PagerSettings _pagerSettings;

        public Pager()
            : this(1, 1, PagerSettings.Default)
        {
        }

        public Pager(int currentPage, int totalPages)
            : this(currentPage, totalPages, PagerSettings.Default)
        {
        }

        public Pager(int currentPage, int totalPages, PagerSettings settings)
        {
            _pagerSettings = settings;
            SetCurrentPage(currentPage, totalPages);
        }

        private static IPagerCalculator _instance = new PagerCalculator();
        private static readonly object _syncRoot = new object();

        public void Init(IPagerCalculator pager)
        {
            _instance = pager;
        }

        public void SetCurrentPage(int currentPage)
        {
            SetCurrentPage(currentPage, _totalPages);
        }

        public void SetCurrentPage(int currentPage, int totalPages)
        {
            if (totalPages < 0) totalPages = 1;
            if (currentPage < 0 || currentPage > totalPages) currentPage = 1;

            _currentPage = currentPage;
            _totalPages = totalPages;
            Calculate();
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
        }

        public int FirstPage
        { get { return 1; } }

        public int PreviousPage
        {
            get { return _previousPage; }
            set { _previousPage = value; }
        }

        public int StartingPage
        {
            get { return _startingPage; }
            set { _startingPage = value; }
        }

        public int EndingPage
        {
            get { return _endingPage; }
            set { _endingPage = value; }
        }

        public int NextPage
        {
            get { return _nextPage; }
            set { _nextPage = value; }
        }

        public int LastPage
        { get { return _totalPages; } }

        public bool IsMultiplePages
        {
            get { return _totalPages > 1; }
        }

        public PagerSettings Settings
        {
            get { return _pagerSettings; }
            set { _pagerSettings = value; }
        }

        public bool CanShowFirst
        {
            get { return (_startingPage != 1); }
        }

        public bool CanShowPrevious
        {
            get { return (_startingPage > 2); }
        }

        public bool CanShowNext
        {
            get { return (_endingPage < (_totalPages - 1)); }
        }

        public bool CanShowLast
        {
            get { return (_endingPage != _totalPages); }
        }

        public void MoveFirst()
        {
            _currentPage = 1;
            Calculate();
        }

        public void MovePrevious()
        {
            _currentPage = _previousPage;
            Calculate();
        }

        public void MoveNext()
        {
            _currentPage = _nextPage;
            Calculate();
        }

        public void MoveLast()
        {
            _currentPage = _totalPages;
            Calculate();
        }

        public void MoveToPage(int selectedPage)
        {
            _currentPage = selectedPage;
            Calculate();
        }

        public void Calculate()
        {
            Calculate(this, _pagerSettings);
        }

        public static void Calculate(Pager pagerData, PagerSettings pagerSettings)
        {
            _instance.Calculate(pagerData, pagerSettings);
        }

        public string ToHtml(Func<int, string> urlBuilder)
        {
            string html = PagerBuilderWeb.Instance.Build(this, this.Settings, urlBuilder);
            return html;
        }

        public string ToHtml(Func<int, string> urlBuilder, PagerSettings settings)
        {
            string html = PagerBuilderWeb.Instance.Build(this, settings, urlBuilder);
            return html;
        }

        public static Pager Get(int currentPage, int totalPages, PagerSettings settings)
        {
            Pager data = new Pager(currentPage, totalPages, settings);
            return data;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}