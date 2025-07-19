namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IServiceBusProductDeletionConsumer : IDisposable
{
    Task ConsumeAsync();
}