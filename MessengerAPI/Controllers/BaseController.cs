using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers;

public class BaseController : ControllerBase
{
    public int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid or missing UserId in token.");
        }
        return userId;
    }
}