namespace Kasi_Server.Common.HTTP;

internal class EmptyCorrelationContextFactory : ICorrelationContextFactory
{
    public string Create() => default;
}