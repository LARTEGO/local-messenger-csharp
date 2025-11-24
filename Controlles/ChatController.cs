using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;

public class ChatController : Controller
{
    public static List<Message> messages = new();

    public IActionResult Index()
    {
        return View(messages);
    }

    
    
}