namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IMessageConsumer
{
    public Task Consume();
}