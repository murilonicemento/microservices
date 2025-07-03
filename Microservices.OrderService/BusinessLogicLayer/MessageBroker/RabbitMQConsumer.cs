using System.Text;
using System.Text.Json;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.MessageBroker.Records;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogicLayer.MessageBroker;

public class RabbitMQConsumer : IMessageConsumer
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQConsumer> _logger;

    public RabbitMQConsumer(IConfiguration configuration, ILogger<RabbitMQConsumer> logger)
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
    }

    public async Task Consume()
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        const string routingKey = "product.update.name";
        const string queueName = "orders.product.update.name.queue";

        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var product = JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);

            _logger.LogInformation("Product name updated: {ProductProductId}, New Name: {ProductProductName}",
                product?.ProductId, product?.ProductName);

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true);
    }
}