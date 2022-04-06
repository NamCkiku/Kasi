namespace Kasi_Server.MessageBrokers;

public interface ICorrelationContextAccessor
{
    object CorrelationContext { get; set; }
}