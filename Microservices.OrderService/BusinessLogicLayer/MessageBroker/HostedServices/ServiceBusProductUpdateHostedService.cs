using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.MessageBroker.HostedServices;

public class ServiceBusProductUpdateHostedService : IHostedService
{
    private readonly IServiceBusProductUpdateConsumer _consumer;

    public ServiceBusProductUpdateHostedService(IServiceBusProductUpdateConsumer consumer)
    {
        _consumer = consumer;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _consumer.ConsumeAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();

        return Task.CompletedTask;
    }
}