namespace BusinessLogicLayer.DTO;

public record OrderResponse(Guid UserId, DateTime OrderDate, List<OrderItemResponse> OrderItems)
{
    public OrderResponse() : this(default, default, default)
    {
    }
}