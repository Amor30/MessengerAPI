using System.ComponentModel.DataAnnotations;

namespace MessengerAPI.Models;

public class User_chats
{
    [Key]
    public int Id_user { get; set; }
    public ApplicationUser User { get; set; }

    [Key]
    public int Id_chat { get; set; }
    public Chat Chat { get; set; }
}