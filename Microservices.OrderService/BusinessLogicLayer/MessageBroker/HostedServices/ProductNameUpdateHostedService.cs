using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.MessageBroker.HostedServices;

public class ProductNameUpdateHostedService : IHostedService
{
    private readonly IMessageUpdateMessageConsumer _productNameUpdateConsumer;

    public ProductNameUpdateHostedService(IMessageUpdateMessageConsumer productNameUpdateConsumer)
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