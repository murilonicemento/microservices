namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IMessageDeletionConsumer
{
    public Task ConsumeAsync();
    public ValueTask DisposeAsync();
}