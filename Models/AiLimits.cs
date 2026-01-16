namespace LocalMessenger.Models
{
    public class AiLimits
    {
        public int MaxInputChars { get; set; }
        public int MaxOutputChars { get; set; }
        public int MaxTokens { get; set; }
        public int MaxTimeoutSeconds { get; set; }
    }
}