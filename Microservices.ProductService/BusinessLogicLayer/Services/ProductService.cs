using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServicesContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessLogicLayer.Services;

public class ProductService : IProductService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public ProductService(
        IValidator<ProductAddRequest> productAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator,
        IMapper mapper,
        IProductRepository productRepository
    )
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        var products = await _productRepository.GetProducts();

        return _mapper.Map<IEnumerable<ProductResponse?>>(products).ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
    {
        var products = await _productRepository.GetProductsByCondition(condition);

        return _mapper.Map<IEnumerable<ProductResponse?>>(products).ToList();
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition)
    {
        var product = await _productRepository.GetProductByCondition(condition);

        return product is null ? null : _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        var validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        ValidateParameters(validationResult);

        var product = _mapper.Map<Product>(productAddRequest);
        var addedProduct = await _productRepository.AddProduct(product);

        return _mapper.Map<ProductResponse>(addedProduct);
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        var validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        ValidateParameters(validationResult);

        var productToUpdate =
            await _productRepository.GetProductByCondition(temp => temp.ProductId == productUpdateRequest.ProductId);

        if (productToUpdate is null)
            throw new ArgumentException("Invalid product ID");

        var product = _mapper.Map<Product>(productToUpdate);
        var updatedProduct = await _productRepository.UpdateProduct(product);

        return _mapper.Map<ProductResponse>(updatedProduct);
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var product = await _productRepository.GetProductByCondition(temp => temp.ProductId == productId);

        if (product is null)
            return false;

        return await _productRepository.DeleteProduct(productId);
    }

    private static void ValidateParameters(ValidationResult validationResult)
    {
        var errorMessages = string.Join(" | ", validationResult.Errors.Select(temp => temp.ErrorMessage));

        throw new ArgumentException(errorMessages);
    }
}