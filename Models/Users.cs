using System.ComponentModel.DataAnnotations;

namespace LocalMessenger.Models
{

public class Users
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required string UserNames { get; set; }
    [Required]
    public required string UserSecondName { get; set; }
    [Required]
    public DateTime DateBirth { get; set; }
    
}
}