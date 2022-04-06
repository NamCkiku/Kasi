namespace Kasi_Server.Utils.Conversions
{
    public interface IConversionTry<in TFrom, TTo>
    {
        bool Is(TFrom from, out TTo to);
    }
}