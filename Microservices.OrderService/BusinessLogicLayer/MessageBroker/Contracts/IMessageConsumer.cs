namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IMessageConsumer
{
    public Task ConsumeAsync();
    public ValueTask DisposeAsync();
}