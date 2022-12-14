using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using ZenBlog.Helpers;

namespace ZenBlog.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]

    public IActionResult Login(string UserName,string Password)
    {
        if(UserName.ToLower() == "quachhuyduc" && Password =="123456")
        {
             HttpContext.Session.SetString("UserName" , "quachhuyduc");
            return RedirectToAction("Index");
        }
        else{
            return View();
        }
      
    }

    
}
