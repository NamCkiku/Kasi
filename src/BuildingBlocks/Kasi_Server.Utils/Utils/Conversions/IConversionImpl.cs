namespace Kasi_Server.Utils.Conversions
{
    public interface IConversionImpl<in TFrom, TTo>
    {
        bool TryTo(TFrom from, out TTo to);
    }
}