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
}