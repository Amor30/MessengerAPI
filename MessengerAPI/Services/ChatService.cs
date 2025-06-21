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

    public async Task<Chat> CreateGroupChat(CreateChatDto createChatDto, int userId)
    {
        var chat = new Chat
        {
            Chat_name = createChatDto.Chat_name,
            Id_type_chat = 2,
            Create_date = DateTime.UtcNow,
            InvitationGuid = Guid.NewGuid()
        };
        
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();
        
        var userChat = new User_chats
        {
            Id_user = userId,
            Id_chat = chat.Id
        };
        _dbContext.UserChats.Add(userChat);
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

    public async Task<Chat?> CreatePersonalChat(CreatePersonalChatDto createPersonalChatDto, int userId)
    {
        var existingChat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Chat_name == createPersonalChatDto.Chat_name);
        if (existingChat != null)
        {
            return existingChat;
        }
        else
        {
            var chat = new Chat
            {
                Chat_name = createPersonalChatDto.Chat_name,
                Id_type_chat = 3,
                Create_date = DateTime.UtcNow,
                InvitationGuid = Guid.NewGuid()
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

            return chat;
        }
    }

    public async Task<bool> AddUserInChat(AddUserInChatDto addUserInChatDto)
    {
        var existingChat = await _dbContext.UserChats.FirstOrDefaultAsync(c => c.Id_user == addUserInChatDto.id_user && c.Id_chat == addUserInChatDto.id_chat);
        if (existingChat == null)
        {
            var User_chat = new User_chats
            {
                Id_chat = addUserInChatDto.id_chat,
                Id_user = addUserInChatDto.id_user
            };

            _dbContext.UserChats.Add(User_chat);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}