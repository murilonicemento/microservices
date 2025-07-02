namespace BusinessLogicLayer.RabbitMQ.Records;

public record ProductNameUpdateMessage(Guid ProductId, string? ProductName);