using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/prompts")]
public class PromptController : ControllerBase
{
    private readonly IPromptStore _store;
    private readonly PromptManager _manager;

    public PromptController(IPromptStore store, PromptManager manager)
    {
        _store = store;
        _manager = manager;
    }

    // 1️⃣ Добавить новый system prompt
    [HttpPost]
    public async Task<IActionResult> Create(SystemPrompt prompt)
    {
        prompt.Id = Guid.NewGuid();
        prompt.UpdateAt = DateTime.Now;
        prompt.IsActive = false;

        await _store.AddAsync(prompt);
        await _store.SaveAsync();

        return Ok(prompt.Id);
    }

    // 2️⃣ Получить все prompt’ы
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _store.GetAllAsync());

    // 3️⃣ Активировать prompt
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _manager.ActivateAsync(id);
        return Ok();
    }
}