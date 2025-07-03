using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.MessageBroker.HostedServices;

public class ProductDeletionMessageHostedService : IHostedService
{
    private readonly IMessageDeletionConsumer _productDeletionConsumer;

    public ProductDeletionMessageHostedService(IMessageDeletionConsumer productDeletionConsumer)
    {
        _productDeletionConsumer = productDeletionConsumer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _productDeletionConsumer.ConsumeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _productDeletionConsumer.DisposeAsync();
    }
}