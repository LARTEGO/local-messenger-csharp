namespace LocalMessenger.Models
{

public class Message
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Text { get; set; }
    public TimeOnly TimeStamp { get; set; }
}
}