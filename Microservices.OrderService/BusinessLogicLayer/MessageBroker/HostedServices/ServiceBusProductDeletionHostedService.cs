using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.MessageBroker.HostedServices;

public class ServiceBusProductDeletionHostedService : IHostedService
{
    private readonly IServiceBusProductDeletionConsumer _productNameDeletionConsumer;

    public ServiceBusProductDeletionHostedService(IServiceBusProductDeletionConsumer consumer)
    {
        _productNameDeletionConsumer = consumer;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _productNameDeletionConsumer.ConsumeAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _productNameDeletionConsumer.Dispose();

        return Task.CompletedTask;
    }
}