﻿using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.MessageBroker.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogicLayer.MessageBroker;

public class RabbitMQProductDeletionConsumer : IMessageDeletionConsumer, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQProductDeletionConsumer> _logger;
    private readonly IDistributedCache _distributedCache;

    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQProductDeletionConsumer(
        IConfiguration configuration,
        ILogger<RabbitMQProductDeletionConsumer> logger,
        IDistributedCache distributedCache
    )
    {
        _configuration = configuration;
        _connectionFactory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ_Host_Name"]!,
            UserName = _configuration["RabbitMQ_User_Name"]!,
            Password = _configuration["RabbitMQ_Password"]!,
            Port = Convert.ToInt16(_configuration["RabbitMQ_Port"])
        };
        _logger = logger;
        _distributedCache = distributedCache;
    }

    public async Task ConsumeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        const string routingKey = "product.delete";
        const string queueName = "orders.product.delete.queue";

        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var product = JsonSerializer.Deserialize<ProductDeletionMessage>(message);

            _logger.LogInformation("Product deleted: {ProductProductId}, Product Name: {ProductProductName}",
                product?.ProductId, product?.ProductName);

            // await HandleProductDelete(product.ProductId);
        };

        await _channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _channel.DisposeAsync();
    }

    private async Task HandleProductDelete(Guid productId)
    {
        var cacheKey = $"product:{productId}";

        await _distributedCache.RemoveAsync(cacheKey);
    }
}