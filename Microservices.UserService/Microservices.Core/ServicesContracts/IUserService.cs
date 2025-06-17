using Microservices.Core.DTO;

namespace Microservices.Core.ServicesContracts;

/// <summary>
/// Contract for users service that contains use cases for users
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Method to handle user registration use case and returns an AuthenticationResponse object that contains status of user registration
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    public Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);

    /// <summary>
    /// Method to handle user login use case and returns an AuthenticationResponse object that contains status of login
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public Task<AuthenticationResponse?> Login(LoginRequest loginRequest);

    /// <summary>
    /// Returns User DTO object based on the given UserId
    /// </summary>
    /// <param name="userId">UserId to search</param>
    /// <returns>User DTO object based on the matching UserId</returns>
    public Task<User?> GetUserByUserId(Guid userId);
}