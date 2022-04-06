namespace Kasi_Server.Utils.Paging
{
    public interface IPagerCalculator
    {
        void Calculate(Pager pager, PagerSettings settings);
    }
}