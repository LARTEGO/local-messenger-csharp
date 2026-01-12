using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;


public class ChatController : Controller
{
    private readonly SettingsBD _db;

    public ChatController(SettingsBD db)
    {
        _db = db;
    }
    




    public async Task<IActionResult> Index()
    {
        var messages = await _db.Messages.OrderBy(m => m.TimeStamp).ToListAsync();
            return View(messages);
    }

    
    
}