using MessengerAPI.Dto;
using MessengerAPI.Models;
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
}