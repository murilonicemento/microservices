namespace BusinessLogicLayer.RabbitMQ.Contracts;

public interface IRabbitMQPublisher
{
    public void Publish<T>(string routingKey, T message);
}