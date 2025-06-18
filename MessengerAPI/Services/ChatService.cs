using Azure.Core;
using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            Create_date = DateTime.UtcNow,
            InvitationGuid = Guid.NewGuid() // Генерация уникальной ссылки на чат
        };
        
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();
        return new CreatedResult($"chat/{chat.Id}", chat);
    }

    public async Task<IActionResult> GetInvitationLink(int id, string baseUrl)
    {
        var chat =  await _dbContext.Chats.FirstOrDefaultAsync(x => x.Id == id);
        if (chat == null)
        {
            return new NotFoundObjectResult(new {Message = $"Chat with id {id} not found"});
        }
        
        var link  =$"{baseUrl.TrimEnd('/')}/join?guid={chat.InvitationGuid}";
        
        return new CreatedResult($"chat/{id}/{link}", link);
    }

    public async Task<IActionResult> JoinChatByLink(Guid guid, int userId)
    {
        var chat = await _dbContext.Chats.FirstOrDefaultAsync();
        if (chat == null)
        {
            return new NotFoundObjectResult(new {Message = "Invalid invitation link"});
        }
        
        var userChat = await _dbContext.UserChats
            .FirstOrDefaultAsync(u => u.Id_user == userId && u.Id_chat == chat.Id);
        if (userChat == null)
         return new BadRequestObjectResult(new {Message = "User is already in chat"});
        
        var result = _dbContext.UserChats.Add(new User_chats { Id_user = userId, Id_chat = chat.Id });
        await _dbContext.SaveChangesAsync();
        return new CreatedResult($"/chats/{chat.Id}", result);
        
    }
}