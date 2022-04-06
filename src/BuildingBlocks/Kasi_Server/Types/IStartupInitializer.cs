namespace Kasi_Server.Types;

public interface IStartupInitializer : IInitializer
{
    void AddInitializer(IInitializer initializer);
}