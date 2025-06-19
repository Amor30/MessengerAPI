using System.Security.Claims;
using MessengerAPI.Dto;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : BaseController
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        try
        {
            var chat = await _chatService.CreateChat(createChatDto);
            return CreatedAtAction(nameof(CreateChat), new { id = chat.Id }, chat);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("link")]
    public async Task<IActionResult> GetInvitationLink(int id)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var link = await _chatService.GetInvitationLink(id, baseUrl);
            return Ok(new { Link = link });
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

    [HttpPost("join")]
    public async Task<IActionResult> JoinChat([FromQuery] Guid guid)
    {
        try
        {
            if (guid == Guid.Empty)
            {
                return BadRequest("Invalid chat link.");
            }

            var userId = GetUserId();
            var result = await _chatService.JoinChatByLink(guid, userId);
            return CreatedAtAction(nameof(JoinChat), new { chatId = result.Id_chat }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("chats")]
    public async Task<IActionResult> GetChat()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null)
        {
            var result = await _chatService.GetChatsByUser(int.Parse(userIdClaim));

            if (result == null || result.Count == 0)
                return new NotFoundObjectResult(new { Message = "The user does not have chats" });
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpPost("personal")]
    public async Task<IActionResult> CreatePersonalChat([FromBody] CreatePersonalChatDto createPersonalChatDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null)
        {
            var userId = int.Parse(userIdClaim);

            var result = await _chatService.CreatePersonalChat(createPersonalChatDto, userId);
            return result;
        }
        return BadRequest();
    }
}