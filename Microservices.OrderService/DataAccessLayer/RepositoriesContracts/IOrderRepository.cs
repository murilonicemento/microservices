﻿using DataAccessLayer.Entities;
using MongoDB.Driver;

namespace DataAccessLayer.RepositoriesContracts;

public interface IOrderRepository
{
    /// <summary>
    /// Retrieves all Orders asynchronously
    /// </summary>
    /// <returns>Returns all orders from the orders collection</returns>
    public Task<IEnumerable<Order>> GetOrders();


    /// <summary>
    /// Retrieves all Orders based on the specified condition asynchronously
    /// </summary>
    /// <param name="filter">The condition to filter orders</param>
    /// <returns>Returning a collection of matching orders</returns>
    public Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);


    /// <summary>
    /// Retrieves a single order based on the specified condition asynchronously
    /// </summary>
    /// <param name="filter">The condition to filter Orders</param>
    /// <returns>Returning matching order</returns>
    public Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);


    /// <summary>
    /// Adds a new Order into the Orders collection asynchronously
    /// </summary>
    /// <param name="order">The order to be added</param>
    /// <returns>Returns the added Order object or null if unsuccessful</returns>
    public Task<Order?> AddOrder(Order order);


    /// <summary>
    /// Updates an existing order asynchronously
    /// </summary>
    /// <param name="order">The order to be added</param>
    /// <returns>Returns the updated order object; or null if not found</returns>
    public Task<Order?> UpdateOrder(Order order);


    /// <summary>
    /// Deletes the order asynchronously
    /// </summary>
    /// <param name="orderId">The Order ID based on which we need to delete the order</param>
    /// <returns>Returns true if the deletion is successful, false otherwise</returns>
    public Task<bool> DeleteOrder(Guid orderId);
}