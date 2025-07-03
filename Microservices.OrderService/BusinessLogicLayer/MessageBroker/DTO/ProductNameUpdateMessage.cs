namespace BusinessLogicLayer.MessageBroker.Records;

public record ProductNameUpdateMessage(Guid ProductId, string? ProductName);