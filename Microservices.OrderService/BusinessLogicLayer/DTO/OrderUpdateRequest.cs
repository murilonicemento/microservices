namespace BusinessLogicLayer.DTO;

public record OrderUpdateRequest(Guid OrderId, Guid UserId, DateTime OrderDate, List<OrderItemUpdateRequest> OrderItems)
{
    public OrderUpdateRequest() : this(default, default, default, default)
    {
    }
}