using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ApplicationUser> CreateUser(UserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Username))
            throw new ArgumentException("Email and Username are required.");

        if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new ApplicationUser
        {
            Email = userDto.Email,
            UserName = userDto.Username,
            Create_date = DateTime.UtcNow,
            Token = string.Empty
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return user;
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
        };

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// ѕолучение списка пользователей чата
    /// </summary>
    /// <param name="idChat">ID чата</param>
    /// <returns>—писок участников чата</returns>

    public async Task<List<ApplicationUser>> GetListUser(int idChat)
    {
        var userChats = _dbContext.UserChats.Where(u => u.Id_chat == idChat);

        List<ApplicationUser> user = [];

        foreach (var us in userChats)
        {
            user.AddRange(_dbContext.Users.Where(u => us.Id_chat == u.Id));
        }
        return await Task.FromResult(user);
    }

    /// <summary>
    /// ѕолучение всех пользователей
    /// </summary>
    /// <returns>список пользователей</returns>

    public async Task<List<ApplicationUser>> GetAllUser()
    {
        var users = await _dbContext.Users.ToListAsync();

        return users;
    }
}