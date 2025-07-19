using System.Text.Json;
using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.MessageBroker.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.MessageBroker;

public class ServiceBusProductDeletionConsumer : IServiceBusProductDeletionConsumer
{
    private readonly ServiceBusProcessor _serviceBusProcessor;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ServiceBusProductDeletionConsumer> _logger;

    public ServiceBusProductDeletionConsumer(
        IDistributedCache distributedCache,
        ILogger<ServiceBusProductDeletionConsumer> logger, ServiceBusClient serviceBusClient
    )
    {
        _distributedCache = distributedCache;
        _logger = logger;

        _serviceBusProcessor = serviceBusClient.CreateProcessor(
            "products.deletions",
            "products.deletions.orders",
            new ServiceBusProcessorOptions { AutoCompleteMessages = false }
        );

        _serviceBusProcessor.ProcessMessageAsync += _serviceBusProcessor_ProcessMessageAsync;
        _serviceBusProcessor.ProcessErrorAsync += _serviceBusProcessor_ProcessErrorAsync;
    }


    private async Task _serviceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        var messageBodyJson = arg.Message.Body.ToString();
        var productDeletionMessage = JsonSerializer.Deserialize<ProductDeletionMessage>(messageBodyJson);

        if (productDeletionMessage != null)
            await HandleProductDeletion(productDeletionMessage);

        // Complete the message to remove it from the queue
        await arg.CompleteMessageAsync(arg.Message);
    }

    private async Task HandleProductDeletion(ProductDeletionMessage productDeletionMessage)
    {
        _logger.LogInformation(
            "ServiceBus: Product deleted: {ProductId}, New name: {ProductName}", productDeletionMessage.ProductId,
            productDeletionMessage.ProductName);

        var cacheKeyToDelete = $"product:{productDeletionMessage.ProductId}";

        await _distributedCache.RemoveAsync(cacheKeyToDelete);
    }

    private Task _serviceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Error while handling message.");
        return Task.CompletedTask;
    }


    public async Task ConsumeAsync()
    {
        await _serviceBusProcessor.StartProcessingAsync();
    }

    public async void Dispose()
    {
        await _serviceBusProcessor.DisposeAsync();
    }
}