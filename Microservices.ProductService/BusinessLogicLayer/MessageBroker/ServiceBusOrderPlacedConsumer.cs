using System.Text.Json;
using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.ServicesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.MessageBroker;

public class ServiceBusOrderPlacedConsumer : IServiceBusOrderPlacedConsumer
{
    private readonly ServiceBusProcessor _serviceBusProcessor;
    private readonly ILogger<ServiceBusOrderPlacedConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ServiceBusOrderPlacedConsumer(ILogger<ServiceBusOrderPlacedConsumer> logger,
        ServiceBusClient serviceBusClient, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

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
        var orderResponseMessage = JsonSerializer.Deserialize<OrderResponse>(messageBodyJson);

        if (orderResponseMessage != null)
        {
            //Child scope
            using var scope = _serviceScopeFactory.CreateScope();
            var productsService = scope.ServiceProvider.GetRequiredService<IProductService>();


            await HandleOrderPlacement(orderResponseMessage, productsService);
        }

        // Complete the message to remove it from the queue
        await arg.CompleteMessageAsync(arg.Message);
    }

    private async Task HandleOrderPlacement(OrderResponse orderResponse, IProductService productsService)
    {
        _logger.LogInformation(
            "ServiceBus: Order Placed: {OrderResponseOrderId}, Order Date: {ToLongDateString} {ToLongTimeString}",
            orderResponse.OrderId, orderResponse.OrderDate.ToLongDateString(),
            orderResponse.OrderDate.ToLongTimeString());

        foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
        {
            var existingProduct =
                await productsService.GetProductByCondition(temp => temp.ProductId == orderItemResponse.ProductId);

            if (existingProduct != null)
            {
                var productUpdateRequest = new ProductUpdateRequest()
                {
                    ProductId = existingProduct.ProductId,
                    ProductName = existingProduct.ProductName,
                    Category = existingProduct.Category,
                    QuantityInStock = existingProduct.QuantityInStock - orderItemResponse.Quantity,
                    UnitPrice = existingProduct.UnitPrice
                };

                await productsService.UpdateProduct(productUpdateRequest);
            }
        }
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