using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using MongoDB.Driver;

namespace BusinessLogicLayer.ServicesContracts;

public interface IOrderService
{
    /// <summary>
    /// Retrieves the list of orders from the orders repository
    /// </summary>
    /// <returns>Returns list of OrderResponse objects</returns>
    public Task<List<OrderResponse?>> GetOrders();


    /// <summary>
    /// Returns list of orders matching with given condition
    /// </summary>
    /// <param name="filter">Expression that represents condition to check</param>
    /// <returns>Returns matching orders as OrderResponse objects</returns>
    public Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);


    /// <summary>
    /// Returns a single order that matches with given condition
    /// </summary>
    /// <param name="filter">Expression that represents the condition to check</param>
    /// <returns>Returns matching order object as OrderResponse; or null if not found</returns>
    public Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);


    /// <summary>
    /// Add (inserts) order into the collection using orders repository
    /// </summary>
    /// <param name="orderAddRequest">Order to insert</param>
    /// <returns>Returns OrderResponse object that contains order details after inserting; or returns null if insertion is unsuccessful.</returns>
    public Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest);


    /// <summary>
    /// Updates the existing order based on the OrderID
    /// </summary>
    /// <param name="orderUpdateRequest">Order data to update</param>
    /// <returns>Returns order object after successful updation; otherwise null</returns>
    public Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest);


    /// <summary>
    /// Deletes an existing order based on given order id
    /// </summary>
    /// <param name="orderId">OrderId to search and delete</param>
    /// <returns>Returns true if the deletion is successful; otherwise false</returns>
    public Task<bool> DeleteOrder(Guid orderId);
}