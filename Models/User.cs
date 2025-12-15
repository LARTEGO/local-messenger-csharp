namespace LocalMessenger.Models
{

public class User
{
    public int Id { get; set; }
    public required string RealName { get; set; }
    public DateTime BDate { get; set; }
}
}