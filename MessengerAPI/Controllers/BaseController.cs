using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    protected int GetUserId()
    {
        return 2;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid or missing UserId in token.");
        }
        return userId;
    }

    protected string? GetClaimValue(string claimType)
    {
        return User.FindFirst(claimType)?.Value;
    }
}