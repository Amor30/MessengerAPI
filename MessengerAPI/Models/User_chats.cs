using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerAPI.Models;

[Table("tb_user_chat")]
public class User_chats
{
    [Key]
    public int Id_user { get; set; }
    public ApplicationUser User { get; set; }

    [Key]
    public int Id_chat { get; set; }
    public Chat Chat { get; set; }
}