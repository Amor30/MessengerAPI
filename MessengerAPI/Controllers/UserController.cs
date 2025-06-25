using MessengerAPI.Controllers;
using MessengerAPI.Dto;
using MessengerAPI.Models;
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
        try
        {
            var user = await _userService.CreateUser(userDto);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, new { user.Id, user.UserName, user.Email });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
{
    try
    {
        var user = await _userService.Authenticate(loginDto);
        return Ok(new { UserId = user.Id, UserName = user.UserName });
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(ex.Message);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
    

    [HttpGet("user_list")]
    public async Task<IActionResult> GetListUser([FromQuery] int idChat)
    {
        try
        {
            var result = await _userService.GetListUser(idChat);
            if (result == null || result.Count == 0)
            {
                return NotFound("� ���� ��� ��� ����������");
            }
            var userDtos = MapToUserDtos(result);
            return Ok(userDtos);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("all_user")]
    public async Task<IActionResult> GetAllUser()
    {
        try
        {
            var result = await _userService.GetAllUser();
            if (result == null || result.Count == 0)
            {
                return NotFound("������ ������������� ����.");
            }
            var userDtos = MapToUserDtos(result);
            return Ok(userDtos);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private List<ListUsersDto> MapToUserDtos(List<ApplicationUser> users)
    {
        var userDtos = new List<ListUsersDto>();
        foreach (var user in users)
        {
            userDtos.Add(new ListUsersDto
            {
                Id = user.Id,
                UserName = user.UserName,
            });
        }
        return userDtos;
    }
}