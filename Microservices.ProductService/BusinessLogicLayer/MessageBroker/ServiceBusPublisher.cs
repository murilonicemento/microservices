using System.Text.Json;
using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.MessageBroker.Contracts;

namespace BusinessLogicLayer.MessageBroker;

public class ServiceBusPublisher : IServiceBusPublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(ServiceBusClient client)
    {
        _sender = client.CreateSender("products.update");
    }

    public async Task Publish<T>(Dictionary<string, object> headers, T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(messageJson);

        foreach (var header in headers)
            serviceBusMessage.ApplicationProperties[header.Key] = header.Value;

        await _sender.SendMessageAsync(serviceBusMessage);
    }
}