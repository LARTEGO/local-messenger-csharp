using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Models;
using System.Collections.Generic;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

public class UserController : Controller
{
  private readonly SettingsBD _db;

    public  UserController(SettingsBD db)
    {
        _db = db;
    }


    

  
    /*public IActionResult Send(string userName, string userSecondName, DateTime DateBirth)
    {
       
         
                Id = users.Count + 1,
                UserNames = userName,
                UserSecondName = userSecondName,
                DateBirth = DateBirth
            });

        return RedirectToAction("Index");
    }*/
}