using System.ComponentModel.DataAnnotations;

namespace LocalMessenger.Models
{

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}
}