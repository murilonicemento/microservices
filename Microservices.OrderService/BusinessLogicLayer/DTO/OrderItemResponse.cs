namespace BusinessLogicLayer.DTO;

public record OrderItemResponse(
    Guid ProductId,
    string? ProductName,
    string? Category,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice)
{
    public OrderItemResponse() : this(default, default, default, default, default, default)
    {
    }
}