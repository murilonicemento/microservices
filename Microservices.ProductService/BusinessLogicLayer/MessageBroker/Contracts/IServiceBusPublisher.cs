namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IServiceBusPublisher
{
    public Task Publish<T>(Dictionary<string, object> headers, T message);
}