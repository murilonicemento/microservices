namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IMessagePublisher
{
    public Task Publish<T>(string routingKey, T message);
}