namespace Kasi_Server.Utils.IdGenerators
{
    public interface IIdGenerator<out T>
    {
        T Create();
    }
}