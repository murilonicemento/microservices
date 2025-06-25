using System.Net;
using System.Net.Http.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients;

public class ProductMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductMicroserviceClient> _logger;

    public ProductMicroserviceClient(HttpClient httpClient, ILogger<ProductMicroserviceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/products/search/product-id/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound => null,
                    HttpStatusCode.BadRequest => throw new HttpRequestException("Bad request", null,
                        HttpStatusCode.BadRequest),
                    _ => throw new HttpRequestException("Request failed;")
                };
            }

            var product = await response.Content.ReadFromJsonAsync<Product>();

            if (product is null)
                throw new ArgumentException("Invalid product ID.");

            return product;
        }
        catch (BulkheadRejectedException exception)
        {
            _logger.LogError(exception, "Bulkhead Isolation blocks the request since the request queue is full.");

            return new Product(
                ProductId: Guid.Empty,
                ProductName: "Temporarily Unavailable. (Bulkhead Isolation)",
                Category: "Temporarily Unavailable. (Bulkhead Isolation)",
                UnitPrice: 0,
                QuantityInStock: 0
            );
        }
    }
}