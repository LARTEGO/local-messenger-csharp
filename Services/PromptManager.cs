using LocalMessenger.Models;


public class PromptManager
{
    private readonly IPromptStore _store;

    public PromptManager(IPromptStore store)
    {
        _store = store;
    }

        public async Task<string> BuildPromptAsync(string userPrompt)
    {
        if (string.IsNullOrWhiteSpace(userPrompt))
            throw new ArgumentException("User prompt is empty");

        var system = await _store.GetActiveAsync();

        return
$"""
SYSTEM:
{system.Content}

USER:
{userPrompt}

ASSISTANT:
""";
    }

  
    public async Task ActivateAsync(Guid id)
    {
        var prompts = await _store.GetAllAsync();

        var target = prompts.FirstOrDefault(p => p.Id == id);
        if (target == null)
            throw new InvalidOperationException("System prompt not found");

        foreach (var p in prompts)
            p.IsActive = false;

        target.IsActive = true;
        target.UpdateAt = DateTime.Now;

        await _store.SaveAsync();
    }

      public Task<SystemPrompt> GetActiveAsync()
        => _store.GetActiveAsync();
}