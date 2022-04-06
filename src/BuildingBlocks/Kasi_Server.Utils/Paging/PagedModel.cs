using MessagePack;

namespace Kasi_Server.Utils.Paging
{
    [MessagePackObject]
    public class PagedModel<T>
    {
        public static readonly PagedModel<T> Empty = new PagedModel<T>(new List<T>(), 0, 1, 1);

        public static PagedModel<T> EmptyModel(long totalCount, int pageIndex, int pageSize) => new PagedModel<T>(new List<T>(), totalCount, pageIndex, pageSize);

        [Key(0)]
        public int CurrentPage { get; set; }

        [Key(1)]
        public int PageSize { get; set; }

        [Key(2)]
        public int TotalPages { get; set; }

        [Key(3)]
        public long TotalCount { get; set; }

        [Key(4)]
        public IList<T> Items { get; set; }

        [Key(5)]
        public object ExtraInfo { get; set; }

        [Key(6)]
        public bool HasPreviousPage => (CurrentPage > 1);

        [Key(7)]
        public bool HasNextPage => (CurrentPage < TotalPages);

        public PagedModel()
        { }

        public PagedModel(IList<T> items, long totalCount, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (items != null && items.Count > 0)
                Items = items;
            else
                Items = new List<T>();
        }

        public PagedList<T> ToList() => new PagedList<T>(Items, TotalCount, CurrentPage, PageSize);
    }
}