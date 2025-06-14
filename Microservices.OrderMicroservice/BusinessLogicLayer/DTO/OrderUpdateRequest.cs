namespace BusinessLogicLayer.DTO;

public record OrderUpdateRequest(Guid UserId, DateTime OrderDate, List<OrderItemUpdateRequest> OrderItems)
{
    public OrderUpdateRequest() : this(default, default, default)
    {
    }
}