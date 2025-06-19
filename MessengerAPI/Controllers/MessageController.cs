using MessengerAPI.Dto;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : BaseController
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageDto messageDto)
    {
        try
        {
            var userId = GetUserId();
            var message = await _messageService.CreateMessage(messageDto, userId);
            return CreatedAtAction(nameof(CreateMessage), new { id = message.Id }, message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        try
        {
            var messages = await _messageService.GetMessages(chatId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}