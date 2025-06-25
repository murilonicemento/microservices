using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;

namespace BusinessLogicLayer.Policies;

public class ProductsMicroservicePolicies : IProductsMicroservicePolicies
{
    private readonly ILogger<ProductsMicroservicePolicies> _logger;

    public ProductsMicroservicePolicies(ILogger<ProductsMicroservicePolicies> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .FallbackAsync(async (context) =>
            {
                _logger.LogWarning("Fallback triggered: The request failed, returning dummy data");

                var product = new Product(
                    ProductId: Guid.Empty,
                    ProductName: "Temporarily  Unavailable (fallback)",
                    Category: "Temporarily Unavailable (fallback)",
                    UnitPrice: 0,
                    QuantityInStock: 0
                );

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json")
                };

                return response;
            });

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy()
    {
        var policy = Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: 2,
            maxQueuingActions: 40,
            onBulkheadRejectedAsync: (context) =>
            {
                _logger.LogWarning("Bulkhead Isolation triggered. Can't send any more requests since the queue is full.");

                throw new BulkheadRejectedException("Bulkhead queue is full.");
            });

        return policy;
    }
}