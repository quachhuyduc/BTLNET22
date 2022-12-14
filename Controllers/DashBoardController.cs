using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;

namespace ZenBlog.Controllers;

public class DashBoardController : Controller
{
    private readonly ILogger<DashBoardController> _logger;

    public DashBoardController(ILogger<DashBoardController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult MyProfile()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
