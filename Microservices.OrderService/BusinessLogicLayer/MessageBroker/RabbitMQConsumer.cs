using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BusinessLogicLayer.MessageBroker;

public class RabbitMQConsumer : IMessageConsumer
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQConsumer(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionFactory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ_Host_Name"]!,
            UserName = _configuration["RabbitMQ_User_Name"]!,
            Password = _configuration["RabbitMQ_Password"]!,
            Port = Convert.ToInt16(_configuration["RabbitMQ_Port"])
        };
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
    }
}