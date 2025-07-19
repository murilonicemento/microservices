namespace BusinessLogicLayer.MessageBroker.Contracts;

public interface IServiceBusOrderPlacedConsumer : IDisposable
{
    public Task ConsumeAsync();
}