using BusinessLogicLayer.RabbitMQ.Contracts;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("RabbitMQ");
        
    }

    public void Publish<T>(string routingKey, T message)
    {
        throw new NotImplementedException();
    }
}