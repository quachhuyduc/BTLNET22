using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;

namespace ZenBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SinglePost()
    {
        return View();
    }
    public IActionResult Categories()
    {
        return View();
    }
     public IActionResult About()
    {
        return View();
    }
 public IActionResult Contact()
    {
        return View();
    }
     public IActionResult Food()
    {
        return View();
    }

 public IActionResult Technology()
    {
        return View();
    }
     public IActionResult Discover()
    {
        return View();
    }
     public IActionResult LifeStyle()
    {
        return View();
    }
     public IActionResult Family()
    {
        return View();
    }
     public IActionResult Startups()
    {
        return View();
    }
      public IActionResult Job()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
