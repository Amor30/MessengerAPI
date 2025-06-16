using System.ComponentModel.DataAnnotations;

namespace MessengerAPI.Models;

public class Type_chat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Name_type { get; set; }
}