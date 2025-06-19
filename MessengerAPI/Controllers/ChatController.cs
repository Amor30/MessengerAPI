using System.Security.Claims;
using Azure.Core;
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
        var result = await _chatService.CreateChat(createChatDto);
        return result;
    }

    [HttpGet("link")]
    public async Task<IActionResult> GetInvitationLink(int id)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        var result = await _chatService.GetInvitationLink(id, baseUrl);
        return result;
    }

    [HttpPost("link")]
    public async Task<IActionResult> JoinChat([FromQuery] Guid guid)
    {
        var userId = GetUserId();
        var result = await _chatService.JoinChatByLink(guid, userId);
        return result;
    }
}