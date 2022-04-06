namespace Kasi_Server.Types;

public interface IIdentifiable<out T>
{
    T Id { get; }
}