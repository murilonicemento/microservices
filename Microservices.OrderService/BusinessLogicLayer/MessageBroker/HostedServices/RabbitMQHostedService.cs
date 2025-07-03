using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.MessageBroker.HostedServices;

public class RabbitMQHostedService : IHostedService
{
    private readonly IMessageConsumer _productNameUpdateConsumer;

    public RabbitMQHostedService(IMessageConsumer productNameUpdateConsumer)
    {
        _productNameUpdateConsumer = productNameUpdateConsumer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _productNameUpdateConsumer.ConsumeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _productNameUpdateConsumer.DisposeAsync();
    }
}