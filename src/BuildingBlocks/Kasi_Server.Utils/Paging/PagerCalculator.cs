namespace Kasi_Server.Utils.Paging
{
    public class PagerCalculator : IPagerCalculator
    {
        #region IPagerCalculator Members

        public void Calculate(Pager pagerData, PagerSettings pagerSettings)
        {
            if (pagerData.CurrentPage < 0) pagerData.CurrentPage = 1;
            if (pagerData.CurrentPage > pagerData.TotalPages) pagerData.CurrentPage = 1;

            int currentPage = pagerData.CurrentPage;

            pagerData.StartingPage = GetStartingPage(pagerData, pagerSettings);
            pagerData.EndingPage = GetEndingPage(pagerData, pagerSettings);

            if (currentPage + 1 <= pagerData.TotalPages)
                pagerData.NextPage = currentPage + 1;
            else
                pagerData.NextPage = currentPage;

            if (currentPage - 1 >= 1)
                pagerData.PreviousPage = currentPage - 1;
            else
                pagerData.PreviousPage = currentPage;
        }

        #endregion IPagerCalculator Members

        #region Private methods

        private static int GetStartingPage(Pager pagerData, PagerSettings settings)
        {
            if (pagerData.CurrentPage <= settings.NumberPagesToDisplay)
            {
                return 1;
            }
            int range = GetRange(pagerData.CurrentPage, settings.NumberPagesToDisplay);
            int totalRanges = GetTotalRanges(pagerData.TotalPages, settings.NumberPagesToDisplay);

            if (range == totalRanges)
            {
                return (pagerData.TotalPages - settings.NumberPagesToDisplay) + 1;
            }
            range--;
            return (range * settings.NumberPagesToDisplay) + 1;
        }

        private static int GetEndingPage(Pager pagerData, PagerSettings settings)
        {
            if (pagerData.TotalPages <= settings.NumberPagesToDisplay)
            {
                return pagerData.TotalPages;
            }

            int range = GetRange(pagerData.CurrentPage, settings.NumberPagesToDisplay);
            int totalRanges = GetTotalRanges(pagerData.TotalPages, settings.NumberPagesToDisplay);

            if (range == totalRanges)
            {
                return pagerData.TotalPages;
            }

            return range * settings.NumberPagesToDisplay;
        }

        private static int GetTotalRanges(int totalPages, int numberPagesToDisplay)
        {
            return GetRange(totalPages, numberPagesToDisplay);
        }

        private static int GetRange(int currentPage, int numberPagesToDisplay)
        {
            if (currentPage <= numberPagesToDisplay) return 1;

            int range = currentPage / numberPagesToDisplay;
            int remainingPages = currentPage % numberPagesToDisplay;

            if (remainingPages > 0)
            {
                range++;
            }

            return range;
        }

        #endregion Private methods
    }
}