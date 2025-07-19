using System.Text.Json;
using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.MessageBroker;

public class ServiceBusProductUpdateConsumer : IServiceBusProductUpdateConsumer
{
    private readonly ServiceBusProcessor _serviceBusProcessor;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ServiceBusProductUpdateConsumer> _logger;

    public ServiceBusProductUpdateConsumer(
        IDistributedCache distributedCache,
        ILogger<ServiceBusProductUpdateConsumer> logger, ServiceBusClient serviceBusClient
    )
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _serviceBusProcessor = serviceBusClient.CreateProcessor(
            "products.update",
            "products.updates.orders",
            new ServiceBusProcessorOptions { AutoCompleteMessages = false }
        );

        _serviceBusProcessor.ProcessMessageAsync += _serviceBusProcessor_ProcessMessageAsync;
        _serviceBusProcessor.ProcessErrorAsync += _serviceBusProcessor_ProcessErrorAsync;
    }

    private async Task _serviceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        var messageBodyJson = arg.Message.Body.ToString();
        var product = JsonSerializer.Deserialize<ProductResponse>(messageBodyJson);

        if (product != null)
            await HandleProductUpdation(product);

        // Complete the message to remove it from the queue
        await arg.CompleteMessageAsync(arg.Message);
    }

    private async Task HandleProductUpdation(ProductResponse product)
    {
        _logger.LogInformation("Product name updated: {ProductProductId}, New name: {ProductProductName}",
            product.ProductId, product.ProductName);

        var productJson = JsonSerializer.Serialize(product);
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        var cacheKeyToWrite = $"product:{product.ProductId}";

        await _distributedCache.SetStringAsync(cacheKeyToWrite, productJson, options);
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