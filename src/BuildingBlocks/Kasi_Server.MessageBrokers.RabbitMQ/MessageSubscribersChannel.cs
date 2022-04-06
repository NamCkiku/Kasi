using System.Threading.Channels;
using Kasi_Server.MessageBrokers.RabbitMQ.Subscribers;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

internal class MessageSubscribersChannel
{
    private readonly Channel<IMessageSubscriber> _channel = Channel.CreateUnbounded<IMessageSubscriber>();

    public ChannelReader<IMessageSubscriber> Reader => _channel.Reader;
    public ChannelWriter<IMessageSubscriber> Writer => _channel.Writer;
}