namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IMessageUpdateMessageConsumer
{
    public Task ConsumeAsync();
    public ValueTask DisposeAsync();
}