namespace Kasi_Server.Common.HTTP;

internal class EmptyCorrelationIdFactory : ICorrelationIdFactory
{
    public string Create() => default;
}