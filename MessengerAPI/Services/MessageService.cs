using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Services;

public class MessageService
{
    private readonly ApplicationDbContext _dbContext;

    public MessageService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(MessageDto messageDto, int UserId)
    {
        var message = new Message
        {
            Msg_text = messageDto.Message,
            Create_date = DateTime.UtcNow,
            Id_chat = messageDto.IdChat,
            Id_user = UserId
        };
        
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        return new CreatedResult($"/chats/{message.Id_chat}", message);
    }

    public async Task<IActionResult> GetMessages(int chatId)
    {
        var messages = _dbContext.Chats.Where(x => x.Id == chatId).ToList();
        return new CreatedResult($"/chats/{chatId}", messages);
    }
}