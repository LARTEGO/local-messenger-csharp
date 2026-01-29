using LocalMessenger.Models;


public interface IPromptStore
{
    Task<SystemPrompt> GetActiveAsync();
    Task<IReadOnlyList<SystemPrompt>> GetAllAsync();
    Task AddAsync(SystemPrompt prompt);
    Task<SystemPrompt?> GetByIdAsync(Guid id);
    Task SaveAsync();
}