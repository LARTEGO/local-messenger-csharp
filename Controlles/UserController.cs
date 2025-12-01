using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;

public class UserController : Controller
{
    private static List<User> users = new();

    public IActionResult Index()
    {
        return View(users);
    }

    [HttpPost]
    public IActionResult Send(string realName, DateTime bDate)
    {
        if (!string.IsNullOrEmpty(realName))
            users.Add(new User {
                Id = users.Count + 1,
                RealName = realName,
                BDate = bDate
            });

        return RedirectToAction("Index");
    }
}