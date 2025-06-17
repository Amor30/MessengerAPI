using MessengerAPI.Dto;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        var (success, result, error) = await _userService.CreateUser(userDto);
        if (!success)
            return BadRequest(new { Message = error });

        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (success, result, error) = await _userService.Login(loginDto);
        if (!success)
            return Unauthorized(new { Message = error });

        return Ok(result);
    }
}