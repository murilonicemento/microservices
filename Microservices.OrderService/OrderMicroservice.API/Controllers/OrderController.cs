using System.Collections.Immutable;
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

        [HttpGet("/search/date/{orderDate:datetime}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByDate(DateTime orderDate)
        {
            var filter = Builders<Order>.Filter.Eq(temp => temp.OrderDate.ToString("yyy-MM-dd"),
                orderDate.ToString("yyy-MM-dd"));
            var orders = await _orderService.GetOrdersByCondition(filter);

            return Ok(orders);
        }
    }
}