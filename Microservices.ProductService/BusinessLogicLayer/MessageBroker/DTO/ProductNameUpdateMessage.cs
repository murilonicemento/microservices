namespace BusinessLogicLayer.MessageBroker.DTO;

public record ProductNameUpdateMessage(Guid ProductId, string? ProductName);