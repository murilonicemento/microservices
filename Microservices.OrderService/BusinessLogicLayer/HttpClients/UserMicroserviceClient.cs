using System.Net;
using System.Net.Http.Json;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.HttpClients;

public class UserMicroserviceClient
{
    private readonly HttpClient _httpClient;

    public UserMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User?> GetUserByUserId(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/user/{userId}");

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

        var user = await response.Content.ReadFromJsonAsync<User>();

        if (user is null)
            throw new ArgumentException("Invalid user ID.");

        return user;
    }
}