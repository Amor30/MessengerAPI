using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Services;

public class ChatService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> CreateChat(CreateChatDto createChatDto)
    {
        var chat = new Chat
        {
            Chat_name = createChatDto.Chat_name,
            Id_type_chat = createChatDto.Id_type_chat,
            Create_date = DateTime.UtcNow
        };
        
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();
        return new CreatedResult($"chat/{chat.Id}", chat);
    }
}