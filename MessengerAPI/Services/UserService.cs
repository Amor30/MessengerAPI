using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace MessengerAPI.Services;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IActionResult> CreateUser(UserDto userDto)
    {
        // Валидация данных
        if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Username))
            return new BadRequestObjectResult(new { Message = "Email and Username are required" });
        
        if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            return new ConflictObjectResult(new { Message = "User with this email already exists" });

        var user = new ApplicationUser
        {
            Email = userDto.Email,
            UserName = userDto.Username,
            Create_date = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            return new BadRequestObjectResult(new { Message = "Failed to create user", Errors = result.Errors });
        
        await _dbContext.SaveChangesAsync();

        return new CreatedResult($"/user/{user.Id}", new { user.Id, user.UserName, user.Email});
    }

    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return new UnauthorizedObjectResult(new { Message = "Invalid email or password" });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
        };

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            signingCredentials: creds
        );
        
        return new OkObjectResult(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        });
    }
}