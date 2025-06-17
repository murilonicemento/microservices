namespace Microservices.Core.DTO;

public record User(Guid UserId, string? Email, string? PersonName, string Gender);