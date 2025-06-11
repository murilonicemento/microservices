using Microservices.Core.DTO;
using Microservices.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest? registerRequest)
        {
            if (registerRequest is null)
                return BadRequest("Invalid registration data.");

            var authenticationResponse = await _userService.Register(registerRequest);

            if (authenticationResponse is null || authenticationResponse.Success == false)
                return BadRequest(authenticationResponse);

            return Ok(authenticationResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest? loginRequest)
        {
            if (loginRequest is null)
                return BadRequest("Invalid login data.");

            var authenticationResponse = await _userService.Login(loginRequest);

            if (authenticationResponse is null || authenticationResponse.Success == false)
                return Unauthorized(authenticationResponse);

            return Ok(authenticationResponse);
        }
    }
}