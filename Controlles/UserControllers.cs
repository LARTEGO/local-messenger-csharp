using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;

using System.Collections.Generic;

public class UserController : Controller
{
    public static List<Users> users = new();

    public IActionResult Index()
    {
        return View(users);
    }

    [HttpPost]
    public IActionResult Send(string userName, string userSecondName, DateTime dateTime)
    {
        if (!string.IsNullOrEmpty(userName))
            users.Add(new Users {
                Id = users.Count + 1,
                UserNames = userName,
                UserSecondName = userSecondName,
                DateBirth = dateTime
            });

        return RedirectToAction("Index");
    }
}