using System.Net;
using System.Net.Http.Json;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.HttpClients;

public class ProductMicroserviceClient
{
    private readonly HttpClient _httpClient;

    public ProductMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product?> GetProductById(Guid productId)
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
}