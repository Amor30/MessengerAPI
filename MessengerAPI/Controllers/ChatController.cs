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
}