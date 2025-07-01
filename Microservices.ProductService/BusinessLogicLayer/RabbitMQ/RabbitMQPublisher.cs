using BusinessLogicLayer.RabbitMQ.Contracts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ_Host_Name"]!,
            UserName = configuration["RabbitMQ_User_Name"]!,
            Password = configuration["RabbitMQ_Password"]!,
            Port = Convert.ToInt16(configuration["RabbitMQ_Port"])
        };
    }

    public async Task Publish<T>(string routingKey, T message)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
    }
}