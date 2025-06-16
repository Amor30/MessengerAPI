using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MessengerAPI.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public DateTime Create_date { get; set; }

    [StringLength(500)]
    public string Token { get; set; }

    // Навигационное свойство для User_chats
    public ICollection<User_chats> UserChats { get; set; } = new List<User_chats>();
}