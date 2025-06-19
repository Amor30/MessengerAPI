using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerAPI.Models;

[Table("tb_message")]
public class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    [Column("Msg_text")]
    public string Msg_text { get; set; }

    [Required]
    [Column("create_date")]
    public DateTime Create_date { get; set; }

    // Внешние ключи
    [Required]
    [Column("id_user")]
    public int Id_user { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    [Column("id_chat")]
    public int Id_chat { get; set; }
    public Chat Chat { get; set; }

    [StringLength(20)]
    [Column("user_name")]
    public string User_name { get; set; }
}