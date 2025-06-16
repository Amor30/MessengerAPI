using System.ComponentModel.DataAnnotations;

namespace MessengerAPI.Models;

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Msg_text { get; set; }

    [Required]
    public DateTime Create_date { get; set; }

    // Внешние ключи
    [Required]
    public int Id_user { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    public int Id_chat { get; set; }
    public Chat Chat { get; set; }

    [StringLength(20)]
    public string User_name { get; set; }
}