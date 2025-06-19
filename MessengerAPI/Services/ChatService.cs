using MessengerAPI.Dto;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MessengerAPI.Services;

public class ChatService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Chat> CreateChat(CreateChatDto createChatDto)
    {
        var chat = new Chat
        {
            Chat_name = createChatDto.Chat_name,
            Id_type_chat = createChatDto.Id_type_chat,
            Create_date = DateTime.UtcNow,
            InvitationGuid = Guid.NewGuid()
        };
        
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();
        return chat;
    }

    public async Task<string> GetInvitationLink(int id, string baseUrl)
    {
        var chat = await _dbContext.Chats.FirstOrDefaultAsync(x => x.Id == id);
        if (chat == null)
        {
            throw new InvalidOperationException($"Chat with id {id} not found");
        }
        
        return $"{baseUrl.TrimEnd('/')}/join?guid={chat.InvitationGuid}";
    }

    public async Task<User_chats> JoinChatByLink(Guid guid, int userId)
    {
        var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.InvitationGuid == guid);
        if (chat == null)
        {
            throw new InvalidOperationException("Invalid invitation link");
        }

        var userChat = await _dbContext.UserChats
            .FirstOrDefaultAsync(u => u.Id_user == userId && u.Id_chat == chat.Id);
        if (userChat != null)
        {
            throw new InvalidOperationException("User is already in chat");
        }

        var newUserChat = new User_chats { Id_user = userId, Id_chat = chat.Id };
        _dbContext.UserChats.Add(newUserChat);
        await _dbContext.SaveChangesAsync();
        return newUserChat;
    }

    public async Task<List<Chat>> GetChatsByUser(int userId)
    {
        var userCharts = _dbContext.UserChats.Where(uc => uc.Id_user == userId);

        List<Chat> chat = [];

        foreach (var user in userCharts)
        {
            chat.AddRange(_dbContext.Chats.Where(uc => user.Id_chat == uc.Id && uc.Id_type_chat == 2));
        }
        return await Task.FromResult(chat);
    }

    public async Task<IActionResult> CreatePersonalChat(CreatePersonalChatDto createPersonalChatDto, int userId)
    {
        var chat = new Chat
        {
            Chat_name = createPersonalChatDto.Chat_name,
            Id_type_chat = 1,
            Create_date = DateTime.UtcNow,
            InvitationGuid = Guid.NewGuid() // Генерация уникальной ссылки на чат
        };

        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();

        var user_chat = new User_chats
        {
            Id_user = createPersonalChatDto.User_id,
            Id_chat = chat.Id
        };

        _dbContext.UserChats.Add(user_chat);
        await _dbContext.SaveChangesAsync();

        var my_chat = new User_chats
        {
            Id_user = userId,
            Id_chat = chat.Id
        };

        _dbContext.UserChats.Add(my_chat);
        await _dbContext.SaveChangesAsync();

        return new CreatedResult($"chat/{chat.Id}", chat);
    }
}