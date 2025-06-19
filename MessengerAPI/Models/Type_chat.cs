using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerAPI.Models;
[Table("tb_type_chat")]
public class Type_chat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Name_type { get; set; }
}