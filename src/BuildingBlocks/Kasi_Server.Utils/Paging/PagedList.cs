namespace Kasi_Server.Utils.Paging
{
    using System;
    using System.Collections.Generic;

    public class PagedList<T> : List<T>
    {
        public static readonly PagedList<T> Empty = new PagedList<T>(new List<T>(), 0, 1, 1);

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public long TotalCount { get; set; }

        public PagedList()
        { }

        public PagedList(IList<T> items, long totalCount, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }

        public bool HasPreviousPage => (CurrentPage > 1);

        public bool HasNextPage => (CurrentPage < TotalPages);

        public PagedModel<T> ToModel() => new PagedModel<T>(this, TotalCount, CurrentPage, PageSize);
    }
}