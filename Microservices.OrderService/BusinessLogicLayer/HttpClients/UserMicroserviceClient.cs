using System.Net;
using System.Net.Http.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace BusinessLogicLayer.HttpClients;

public class UserMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserMicroserviceClient> _logger;

    public UserMicroserviceClient(HttpClient httpClient, ILogger<UserMicroserviceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<User?> GetUserByUserId(Guid userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new HttpRequestException("Bad request", null,
                        HttpStatusCode.BadRequest);

                return new User
                (
                    PersonName: "Temporarily Unavailable.",
                    Email: "Temporarily Unavailable.",
                    Gender: "Temporarily Unavailable.",
                    UserId: Guid.Empty
                );
            }

            var user = await response.Content.ReadFromJsonAsync<User>();

            if (user is null)
                throw new ArgumentException("Invalid user ID.");

            return user;
        }
        catch (BrokenCircuitException exception)
        {
            _logger.LogError(exception,
                "Request failed because of circuit breaker is in Open state. Returning dummy data.");

            return new User
            (
                PersonName: "Temporarily Unavailable.",
                Email: "Temporarily Unavailable.",
                Gender: "Temporarily Unavailable.",
                UserId: Guid.Empty
            );
        }
    }
}