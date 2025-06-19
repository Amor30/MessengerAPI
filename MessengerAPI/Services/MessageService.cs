using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerAPI.Services;

public class MessageService
{
    private readonly ApplicationDbContext _dbContext;

    public MessageService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Message> CreateMessage(MessageDto messageDto, int userId)
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
        return message;
    }

    public async Task<List<Message>> GetMessages(int chatId)
    {
        return await _dbContext.Messages
            .Where(m => m.Id_chat == chatId)
            .ToListAsync();
    }
}