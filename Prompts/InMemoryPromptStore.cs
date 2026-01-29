using LocalMessenger.Models;


public class InMemoryPromptStore : IPromptStore
{
    private readonly List<SystemPrompt> _prompts = new();

    public InMemoryPromptStore()
    {
        _prompts.Add(new SystemPrompt
        {
            Id = Guid.NewGuid(),
            Name = "default",
            Content = "You are a concise assistant. Answer briefly.",
            IsActive = true,
            UpdateAt = DateTime.Now
        });
    }

    public Task<SystemPrompt> GetActiveAsync()
        => Task.FromResult(_prompts.First(p => p.IsActive));

    public Task<IReadOnlyList<SystemPrompt>> GetAllAsync()
        => Task.FromResult((IReadOnlyList<SystemPrompt>)_prompts.AsReadOnly());

    public Task<SystemPrompt?> GetByIdAsync(Guid id)
        => Task.FromResult(_prompts.FirstOrDefault(p => p.Id == id));

    public Task AddAsync(SystemPrompt prompt)
    {
        _prompts.Add(prompt);
        return Task.CompletedTask;
    }

    public Task SaveAsync() => Task.CompletedTask;
}