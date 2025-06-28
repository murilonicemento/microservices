using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients;

public class ProductMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductMicroserviceClient> _logger;
    private readonly IDistributedCache _distributedCache;

    public ProductMicroserviceClient(HttpClient httpClient, ILogger<ProductMicroserviceClient> logger,
        IDistributedCache distributedCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _distributedCache = distributedCache;
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        try
        {
            var cacheKey = $"product:{productId}";
            var cachedProduct = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedProduct is not null)
                return JsonSerializer.Deserialize<Product>(cachedProduct);

            var response = await _httpClient.GetAsync($"/api/products/search/product-id/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    var productFallback = await response.Content.ReadFromJsonAsync<Product>();

                    if (productFallback is null)
                        throw new NotImplementedException("Fallback policy was not implemented.");

                    return productFallback;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new HttpRequestException("Bad request", null,
                        HttpStatusCode.BadRequest);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new HttpRequestException("Request failed");
            }

            var product = await response.Content.ReadFromJsonAsync<Product>();

            if (product is null)
                throw new ArgumentException("Invalid product ID.");

            var productJson = JsonSerializer.SerializeToUtf8Bytes(product);

            var distributedCacheEntryOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            await _distributedCache.SetAsync(cacheKey, productJson, distributedCacheEntryOptions);

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