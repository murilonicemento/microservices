using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.ServicesContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
    private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
    private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
    private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
    private readonly UserMicroserviceClient _userMicroserviceClient;
    private readonly ProductMicroserviceClient _productMicroserviceClient;

    public OrderService(
        IOrderRepository ordersRepository,
        IMapper mapper,
        IValidator<OrderAddRequest> orderAddRequestValidator,
        IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
        IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
        IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator,
        UserMicroserviceClient userMicroserviceClient,
        ProductMicroserviceClient productMicroserviceClient)
    {
        _orderRepository = ordersRepository;
        _mapper = mapper;
        _orderAddRequestValidator = orderAddRequestValidator;
        _orderItemAddRequestValidator = orderItemAddRequestValidator;
        _orderUpdateRequestValidator = orderUpdateRequestValidator;
        _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
        _userMicroserviceClient = userMicroserviceClient;
        _productMicroserviceClient = productMicroserviceClient;
    }

    public async Task<List<OrderResponse?>> GetOrders()
    {
        var orders = await _orderRepository.GetOrders();

        return _mapper.Map<IEnumerable<OrderResponse?>>(orders).ToList();
    }

    public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        var orders = await _orderRepository.GetOrdersByCondition(filter);

        return _mapper.Map<IEnumerable<OrderResponse?>>(orders).ToList();
    }

    public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        var order = await _orderRepository.GetOrderByCondition(filter);

        return order is null
            ? null
            : _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
    {
        var validationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);

        ValidateParameters(validationResult);

        foreach (var orderItemAddRequest in orderAddRequest.OrderItems)
        {
            var orderItemValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);

            ValidateParameters(orderItemValidationResult);

            var product = await _productMicroserviceClient.GetProductById(orderItemAddRequest.ProductId);

            if (product is null)
                throw new ArgumentException("Invalid Product ID.");
        }

        var user = await _userMicroserviceClient.GetUserByUserId(orderAddRequest.UserId);

        if (user is null)
            throw new ArgumentException("Invalid User Id.");

        var order = _mapper.Map<Order>(orderAddRequest);

        foreach (var orderItem in order.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        order.TotalBill = order.OrderItems.Sum(temp => temp.TotalPrice);

        var addedOrder = await _orderRepository.AddOrder(order);

        return addedOrder is null
            ? null
            : _mapper.Map<OrderResponse>(addedOrder);
    }

    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
    {
        var validationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);

        ValidateParameters(validationResult);

        foreach (var orderItemUpdateRequest in orderUpdateRequest.OrderItems)
        {
            var orderItemValidationResult =
                await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);

            ValidateParameters(orderItemValidationResult);
        }

        var user = await _userMicroserviceClient.GetUserByUserId(orderUpdateRequest.UserId);

        if (user is null)
            throw new ArgumentException("Invalid User Id.");

        var order = _mapper.Map<Order>(orderUpdateRequest);

        foreach (var orderItem in order.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        order.TotalBill = order.OrderItems.Sum(temp => temp.TotalPrice);

        var updatedOrder = await _orderRepository.UpdateOrder(order);

        return updatedOrder is null
            ? null
            : _mapper.Map<OrderResponse>(updatedOrder);
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var filter = Builders<Order>.Filter.Eq(temp => temp.OrderId, orderId);

        var order = await _orderRepository.GetOrderByCondition(filter);

        if (order is null)
            return false;

        return await _orderRepository.DeleteOrder(orderId);
    }

    private static void ValidateParameters(ValidationResult validationResult)
    {
        if (validationResult.IsValid) return;

        var errorMessages = string.Join(" | ", validationResult.Errors.Select(temp => temp.ErrorMessage));

        throw new ArgumentException(errorMessages);
    }
}