using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MessengerAPI.Models;
[Table("tb_user")]
public class ApplicationUser : IdentityUser<int>
{
    [Required]
    public DateTime Create_date { get; set; }

    [StringLength(500)]
    public string Token { get; set; }

    public ICollection<User_chats> UserChats { get; set; } = new List<User_chats>();
}