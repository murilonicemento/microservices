namespace BusinessLogicLayer.RabbitMQ.Contracts;

public interface IRabbitMQPublisher
{
    public Task Publish<T>(string routingKey, T message);
}