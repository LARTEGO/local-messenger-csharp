using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;

public class UserController : Controller
{
    private static List<Message> messages = new();

    public IActionResult Index()
    {
        return View(messages);
    }

    [HttpPost]
    public IActionResult Send(string userName, string text)
    {
        if (!string.IsNullOrEmpty(text))
            messages.Add(new Message {
                Id = messages.Count + 1,
                UserName = userName,
                Text = text,
                TimeStamp = DateTime.Now
            });

        return RedirectToAction("Index");
    }
}