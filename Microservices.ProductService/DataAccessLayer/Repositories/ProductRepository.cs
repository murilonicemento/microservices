using System.Linq.Expressions;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
    {
        return await _dbContext.Products.Where(condition).ToListAsync();
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(condition);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        var existingProduct =
            await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductId == product.ProductId);

        if (existingProduct is null)
            return null;

        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.UnitPrice = product.UnitPrice;

        await _dbContext.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(product => product.ProductId == productId);

        if (existingProduct is null)
            return false;

        _dbContext.Products.Remove(existingProduct);

        var rowsAffected = await _dbContext.SaveChangesAsync();

        return rowsAffected > 0;
    }
}