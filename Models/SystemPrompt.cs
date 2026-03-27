namespace LocalMessenger.Models
{
    public class SystemPrompt
    {
        public Guid Id { get; set;}
        public string Name {get; set;}
        public string Content {get;set;}
        public bool IsActive { get; set; }
        public DateTime UpdateAt {get; set;}
    }
}