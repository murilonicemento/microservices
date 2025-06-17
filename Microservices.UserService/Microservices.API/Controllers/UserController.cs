using Microservices.Core.DTO;
using Microservices.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<User>> GetUserByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Invalid user id.");

        var user = await _userService.GetUserByUserId(userId);

        if (user is null)
            return NotFound(user);

        return Ok(user);
    }
}