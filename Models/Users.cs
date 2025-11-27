namespace LocalMessenger.Models
{

public class Users
{
    public int Id { get; set; }
    public required string UserNames { get; set; }
    public required string UserSecondName { get; set; }
    public DateTime DateBirth { get; set; }
    
}
}