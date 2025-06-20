using MessengerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Services
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!db.TypeChats.Any())
            {
                await SeedTypeChat(db);
            }
            var idPublicChat = await db.TypeChats
                .Where(t => t.Name_type == "Общий")
                .Select(t => t.Id)
                .FirstOrDefaultAsync();
            if (!await db.Chats.AnyAsync(c => c.Id_type_chat == idPublicChat))
            {
                await SeedPublicChat(db, idPublicChat);
            }
        }
        public static async Task SeedTypeChat(ApplicationDbContext db)
        {
            var defoultTypeChat = new List<Type_chat>
            {
                new Type_chat {Name_type = "Общий"},
                new Type_chat {Name_type = "Групповой"},
                new Type_chat {Name_type = "Личный"}
            };
            await db.TypeChats.AddRangeAsync(defoultTypeChat);
            await db.SaveChangesAsync();
        }

        public static async Task SeedPublicChat(ApplicationDbContext db, int idPublicChat)
        {
            var insertPublicChat = new List<Chat>
            {
                new Chat { Chat_name = "Общий чат", Create_date = DateTime.Now, Id_type_chat = idPublicChat}
            };
            await db.Chats.AddRangeAsync(insertPublicChat);
            await db.SaveChangesAsync();
        }
    }
}
