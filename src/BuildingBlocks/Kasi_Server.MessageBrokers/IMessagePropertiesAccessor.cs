namespace Kasi_Server.MessageBrokers;

public interface IMessagePropertiesAccessor
{
    IMessageProperties MessageProperties { get; set; }
}