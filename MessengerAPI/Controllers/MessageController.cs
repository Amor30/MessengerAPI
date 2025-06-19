using MessengerAPI.Dto;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : BaseController
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    
    public async Task<IActionResult> CreateMessage(MessageDto messageDto)
    {
        var userId = GetUserId();
        var result = await _messageService.CreateMessage(messageDto, userId);
        return result;
    }

    public async Task<IActionResult> GetMessages(int chatId)
    {
        var result = await _messageService.GetMessages(chatId);
        return result;
    }
}