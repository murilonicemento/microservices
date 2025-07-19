namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IServiceBusProductUpdateConsumer : IDisposable
{
    public Task ConsumeAsync();
}