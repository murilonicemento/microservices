using Microservices.Core.DTO;
using Microservices.Core.Entities;
using Microservices.Core.RepositoriesContracts;
using Microservices.Core.ServicesContracts;

namespace Microservices.Core.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var user = new ApplicationUser
        {
            Email = registerRequest.Email,
            Password = registerRequest.Password,
            PersonName = registerRequest.PersonName,
            Gender = registerRequest.Gender.ToString()
        };

        var registeredUser = await _userRepository.AddUser(user);

        return registeredUser is null
            ? null
            : new AuthenticationResponse(registeredUser.UserId, registeredUser.Email, registeredUser.PersonName,
                registeredUser.Gender, "token", true);
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        return user is null
            ? null
            : new AuthenticationResponse(user.UserId, user.Email, user.PersonName, user.Gender, "token", true);
    }
}