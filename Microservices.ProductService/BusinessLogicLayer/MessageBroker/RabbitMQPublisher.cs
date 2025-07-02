using System.Text.Json;
using BusinessLogicLayer.MessageBroker.Contracts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BusinessLogicLayer.MessageBroker;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQPublisher(IConfiguration configuration)
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

    public async Task Publish<T>(string routingKey, T message)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

        var messageBody = JsonSerializer.SerializeToUtf8Bytes(message);

        await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: messageBody);
    }
}