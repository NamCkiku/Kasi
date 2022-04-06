namespace Kasi_Server.Utils.Paging
{
    public static class PagedTools
    {
        public static long Page<T>(int size, Func<long> total, Func<int, int, IList<T>> data, Action<IList<T>> process)
        {
            var count = 0;
            var totalCount = total.Invoke();
            var pageCount = (int)Math.Ceiling(totalCount / (double)size);

            for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            {
                var page = data.Invoke(size, currentPage);

                if (page != null) count += page.Count;

                process.Invoke(page);
            }

            return count;
        }

        public static async Task<long> PageAsync<T>(int size,
            Func<CancellationToken, Task<long>> total,
            Func<int, int, CancellationToken, Task<IList<T>>> data,
            Func<IList<T>, CancellationToken, Task> process,
            CancellationToken cancellationToken = default)
        {
            var count = 0;
            var totalCount = await total.Invoke(cancellationToken);
            var pageCount = (int)Math.Ceiling(totalCount / (double)size);

            for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            {
                var page = await data.Invoke(size, currentPage, cancellationToken);

                if (page != null) count += page.Count;

                await process.Invoke(page, cancellationToken);
            }

            return count;
        }

        public static long Offset<T>(int limit, Func<int, int, IList<T>> data, Action<IList<T>> process)
        {
            var total = 0;
            var skip = 0;

            while (true)
            {
                var _data = data.Invoke(skip, limit);

                if (_data == null || _data.Count == 0) return total;

                total += _data.Count;

                process.Invoke(_data);

                skip += limit;
            }
        }

        public static async Task<long> OffsetAsync<T>(int limit,
            Func<int, int, CancellationToken, Task<IList<T>>> data,
            Func<IList<T>, CancellationToken, Task> process,
            CancellationToken cancellationToken = default)
        {
            var total = 0;
            var skip = 0;

            while (true)
            {
                var _data = await data.Invoke(skip, limit, cancellationToken);

                if (_data == null || _data.Count == 0) return total;

                total += _data.Count;

                await process.Invoke(_data, cancellationToken);

                skip += limit;
            }
        }
    }
}