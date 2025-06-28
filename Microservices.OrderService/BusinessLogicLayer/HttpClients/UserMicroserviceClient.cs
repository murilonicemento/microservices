using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace BusinessLogicLayer.HttpClients;

public class UserMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserMicroserviceClient> _logger;
    private readonly IDistributedCache _distributedCache;

    public UserMicroserviceClient(HttpClient httpClient, ILogger<UserMicroserviceClient> logger,
        IDistributedCache distributedCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _distributedCache = distributedCache;
    }

    public async Task<User?> GetUserByUserId(Guid userId)
    {
        try
        {
            var cacheKey = $"user:{userId}";
            var cachedUser = await _distributedCache.GetAsync(cacheKey);

            if (cachedUser is not null)
                return JsonSerializer.Deserialize<User>(cachedUser);

            var response = await _httpClient.GetAsync($"/api/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    var userFallback = await response.Content.ReadFromJsonAsync<User>();

                    if (userFallback is null)
                        throw new NotImplementedException();

                    return userFallback;
                }

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

            var serializedUser = JsonSerializer.SerializeToUtf8Bytes(user);
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

            await _distributedCache.SetAsync($"user:{userId}", serializedUser, distributedCacheEntryOptions);

            return user;
        }
        catch (BrokenCircuitException exception)
        {
            _logger.LogError(exception,
                "Request failed because of circuit breaker is in Open state. Returning dummy data.");

            return new User
            (
                PersonName: "Temporarily Unavailable. (Circuit breaker)",
                Email: "Temporarily Unavailable. (Circuit breaker)",
                Gender: "Temporarily Unavailable. (Circuit breaker)",
                UserId: Guid.Empty
            );
        }
        catch (TimeoutRejectedException exception)
        {
            _logger.LogError(exception, "Timeout occurred while fetching data. Return dummy data.");

            return new User
            (
                PersonName: "Temporarily Unavailable. (Timeout)",
                Email: "Temporarily Unavailable. (Timeout)",
                Gender: "Temporarily Unavailable. (Timeout)",
                UserId: Guid.Empty
            );
        }
    }
}