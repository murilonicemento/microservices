namespace BusinessLogicLayer.MessageBroker.DTO;

public record ProductDeletionMessage(Guid ProductId, string? ProductName);