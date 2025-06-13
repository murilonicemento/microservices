using System.Linq.Expressions;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.ServicesContracts;

public interface IProductService
{
    /// <summary>
    /// Retrieves the list of products from the products repository
    /// </summary>
    /// <returns>Returns list of</returns>
    public Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves list of products matching with given condition
    /// </summary>
    /// <param name="condition">Expression that represents condition to check</param>
    /// <returns>Returns match products</returns>
    public Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Retrieves a product matching with given condition
    /// </summary>
    /// <param name="condition">Expression that represents condition to check</param>
    /// <returns>Returns match product</returns>
    public Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Adds (inserts) product into the table using products repository
    /// </summary>
    /// <param name="productAddRequest">Product to insert</param>
    /// <returns>Product after inserting or null if unsuccessful</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);


    /// <summary>
    /// Updates the existing product based on the ProductID
    /// </summary>
    /// <param name="productUpdateRequest">Product data to update</param>
    /// <returns>Returns product object after successful update; otherwise null</returns>
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);


    /// <summary>
    /// Deletes an existing product based on given product id
    /// </summary>
    /// <param name="productId">ProductID to search and delete</param>
    /// <returns>Returns true if the deletion is successful; otherwise false</returns>
    Task<bool> DeleteProduct(Guid productId);
}