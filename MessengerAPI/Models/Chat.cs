using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerAPI.Models;
[Table("tb_chat")]
public class Chat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    [Column("chat_name")]
    public string Chat_name { get; set; }

    [Required]
    [Column("create_date")]
    public DateTime Create_date { get; set; }

    // Внешний ключ для Type_Chat
    [Required]
    [Column("id_type_chat")]
    public int Id_type_chat { get; set; }
    public Type_chat TypeChat { get; set; }

    [Column("guid")]
    public Guid InvitationGuid { get; set; }
    // Навигационное свойство для User_chats
    [NotMapped]
    public ICollection<User_chats> UserChats { get; set; } = new List<User_chats>();

    // Навигационное свойство для Messages
    [NotMapped]
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}