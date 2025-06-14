using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories;

public class OrderRepository : IOrderRepository
{
    private const string CollectionName = "orders";
    private readonly IMongoCollection<Order> _ordersCollection;

    public OrderRepository(IMongoDatabase mongoDatabase)
    {
        _ordersCollection = mongoDatabase.GetCollection<Order>(CollectionName);
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        var filter = Builders<Order>.Filter.Empty;

        return await (await _ordersCollection.FindAsync(filter)).ToListAsync();
    }

    public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        return await (await _ordersCollection.FindAsync(filter)).ToListAsync();
    }

    public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        return (await _ordersCollection.FindAsync(filter)).FirstOrDefault();
    }

    public async Task<Order?> AddOrder(Order order)
    {
        order.OrderId = Guid.NewGuid();

        await _ordersCollection.InsertOneAsync(order);

        return order;
    }

    public async Task<Order?> UpdateOrder(Order order)
    {
        var filter = Builders<Order>.Filter.Eq(temp => temp.OrderId, order.OrderId);
        var existingOrder = (await _ordersCollection.FindAsync(filter)).FirstOrDefault();

        if (existingOrder is null)
            return null;

        var result = await _ordersCollection.ReplaceOneAsync(filter, order);

        return result.ModifiedCount > 0 ? order : null;
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var filter = Builders<Order>.Filter.Eq(temp => temp.OrderId, orderId);
        var order = (await _ordersCollection.FindAsync(filter)).FirstOrDefault();

        if (order is null)
            return false;

        var result = await _ordersCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}