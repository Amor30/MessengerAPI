using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Identity;
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

    public async Task<(bool Success, object Result, string ErrorMessage)> CreateUser(UserDto userDto)
    {
        // Валидация данных
        if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Username))
            return (false, null, "Email and Username are required.");

        if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 6)
            return (false, null, "Password must be at least 6 characters long.");

        var user = new ApplicationUser
        {
            Email = userDto.Email,
            UserName = userDto.Username,
            Create_date = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            return (false, null, string.Join(", ", result.Errors.Select(e => e.Description)));
        
        await _dbContext.SaveChangesAsync();

        return (true, new { Id = user.Id, UserName = user.UserName, Email = user.Email }, null);
    }

    public async Task<(bool Success, object Result, string ErrorMessage)> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return (false, null, "Invalid email or password");

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

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (true, new { Token = tokenString }, null);
    }
}