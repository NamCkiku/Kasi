namespace Kasi_Server.Utils.Paging
{
    using System;

    public interface IPagerBuilderWeb
    {
        string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder);

        string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder);

        string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder);
    }
}