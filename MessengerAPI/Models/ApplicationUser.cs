using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MessengerAPI.Models;
[Table("tb_user")]
public class ApplicationUser : IdentityUser<int>
{
    [Column("user_name")]
    public override string? UserName { get; set; }
    [Column("email")]
    public override string? Email { get; set; }
    [Column("password")]
    public override string? PasswordHash { get; set; }

    [Required]
    [Column("create_date")]
    public DateTime Create_date { get; set; }

    [StringLength(500)]
    [Column("token")]
    public string Token { get; set; }

    [NotMapped]
    public ICollection<User_chats> UserChats { get; set; } = new List<User_chats>();
}