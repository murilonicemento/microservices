namespace BusinessLogicLayer.DTO;

public record Product(Guid ProductId, string? ProductName, string? Category, double UnitPrice, int QuantityInStock);