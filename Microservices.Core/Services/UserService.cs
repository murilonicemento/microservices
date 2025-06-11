using AutoMapper;
using Microservices.Core.DTO;
using Microservices.Core.Entities;
using Microservices.Core.RepositoriesContracts;
using Microservices.Core.ServicesContracts;

namespace Microservices.Core.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var user = _mapper.Map<ApplicationUser>(registerRequest);

        var registeredUser = await _userRepository.AddUser(user);

        return registeredUser is null
            ? null
            : _mapper.Map<AuthenticationResponse>(registeredUser) with { Success = true, Token = "token" };
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        return user is null
            ? null
            : _mapper.Map<AuthenticationResponse>(user) with { Success = true, Token = "token" };
    }
}