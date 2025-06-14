using System.Collections.Immutable;
using System.Threading.Channels;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServicesContracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace OrderMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        [HttpGet("/search/order-id/{orderId:guid}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid orderId)
        {
            var filter = Builders<Order>.Filter.Eq(temp => temp.OrderId, orderId);
            var order = await _orderService.GetOrderByCondition(filter);

            if (order is null)
                return NotFound("Order not exist.");

            return Ok(order);
        }

        [HttpGet("/search/product-id/{productId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByProduct(Guid productId)
        {
            var orderItemFilter = Builders<OrderItem>.Filter.Eq(tempProduct => tempProduct.ProductId, productId);
            var filter = Builders<Order>.Filter.ElemMatch(temp => temp.OrderItems, orderItemFilter);
            var orders = await _orderService.GetOrdersByCondition(filter);

            return Ok(orders);
        }

        [HttpGet("/search/order-date/{orderDate:datetime}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByDate(DateTime orderDate)
        {
            var filter = Builders<Order>.Filter.Eq(temp => temp.OrderDate.ToString("yyy-MM-dd"),
                orderDate.ToString("yyy-MM-dd"));
            var orders = await _orderService.GetOrdersByCondition(filter);

            return Ok(orders);
        }

        [HttpGet("/search/user-id/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByUserId(Guid userId)
        {
            var filter = Builders<Order>.Filter.Eq(temp => temp.UserId, userId);
            var orders = await _orderService.GetOrdersByCondition(filter);

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> AddOrder(OrderAddRequest? orderAddRequest)
        {
            if (orderAddRequest is null)
                return BadRequest("Invalid order data");

            var orderResponse = await _orderService.AddOrder(orderAddRequest);

            return orderResponse is null
                ? Problem("Error in adding product")
                : Created($"api/Orders/search/order-id/{orderResponse?.OrderId}", orderResponse);
        }

        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> UpdateOrder(OrderUpdateRequest? orderUpdateRequest,
            [FromRoute] Guid orderId)
        {
            if (orderUpdateRequest is null)
                return BadRequest("Invalid order data");

            var orderResponse = await _orderService.UpdateOrder(orderUpdateRequest);

            return orderResponse is null
                ? Problem("Error in updating product")
                : Ok(orderResponse);
        }

        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> DeleteOrder(Guid orderId)
        {
            if (orderId != Guid.Empty)
                return BadRequest("Invalid order data");

            var isDeleted = await _orderService.DeleteOrder(orderId);

            return !isDeleted
                ? Problem("Error in deleting product")
                : Ok(true);
        }
    }
}