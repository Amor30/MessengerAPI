using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Services;

public class MessageService
{
    private readonly ApplicationDbContext _dbContext;

    public MessageService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> CreateMessage(MessageDto messageDto, int userId)
    {
        var message = new Message
        {
            Msg_text = messageDto.Message,
            Create_date = DateTime.UtcNow,
            Id_chat = messageDto.IdChat,
            Id_user = userId
        };
        
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        return new CreatedResult($"/chats/{message.Id}", message);
    }

    public async Task<IActionResult> GetMessages(int chatId)
    {
        var messages = await _dbContext.Messages
            .Where(m => m.Id_chat == chatId)
            .ToListAsync();
        return new CreatedResult($"/chats/{chatId}", messages);
    }
}