using System.ComponentModel.DataAnnotations;

namespace MessengerAPI.Models;

public class Chat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Chat_name { get; set; }

    [Required]
    public DateTime Create_date { get; set; }

    // Внешний ключ для Type_Chat
    [Required]
    public int Id_type_chat { get; set; }
    public Type_chat TypeChat { get; set; }

    // Навигационное свойство для User_chats
    public ICollection<User_chats> UserChats { get; set; } = new List<User_chats>();

    // Навигационное свойство для Messages
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}