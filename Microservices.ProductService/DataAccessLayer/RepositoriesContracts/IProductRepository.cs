using System.Linq.Expressions;
using DataAccessLayer.Entities;

namespace DataAccessLayer.RepositoriesContracts;

/// <summary>
/// Represents a repository for managing Products table 
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products asynchronously
    /// </summary>
    /// <returns>Returns all products from the table</returns>
    public Task<IEnumerable<Product>> GetProducts();

    /// <summary>
    /// Retrieves all products based on the specified condition asynchronously
    /// </summary>
    /// <param name="condition">The condition to filter products</param>
    /// <returns>Returning a collection of matching products</returns>
    public Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Retrieves a single product based on the specified condition asynchronously
    /// </summary>
    /// <param name="condition">The condition to filter products</param>
    /// <returns>Returning a collection of matching products</returns>
    public Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Adds a new product into the products table asynchronously
    /// </summary>
    /// <param name="product">The product to be added</param>
    /// <returns>Returns the added product object or null if unsuccessful</returns>
    public Task<Product?> AddProduct(Product product);

    /// <summary>
    /// Update a product into the products table asynchronously
    /// </summary>
    /// <param name="product">The product to be updated</param>
    /// <returns>Returns the updated product object or null if unsuccessful</returns>
    public Task<Product?> UpdateProduct(Product product);

    /// <summary>
    /// Deletes the product asynchronously
    /// </summary>
    /// <param name="productId">The product ID to be deleted</param>
    /// <returns>Returns true if the deletion is successfull, false otherwise</returns>
    public Task<bool> DeleteProduct(Guid productId);
}