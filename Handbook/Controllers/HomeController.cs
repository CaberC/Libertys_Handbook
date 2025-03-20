using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;

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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult FAQ()
    {
        return View();
    }
    public IActionResult Translation()
    {
        return View();
    }
    public IActionResult Database()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpGet]
    public IActionResult Member()
    {
        
        ViewData["Title"] = "Member Page";
        Models.Date day = new Date();
        day.Today();
        ViewData["Day"] = day.ToString();
        ViewData["body"]=PersonController.getUsers();
        
        return View(@"Person/Member");
    }
    [HttpPost]
    public IActionResult MemberLogin()
    {
        
        ViewData["Title"] = "HomePage";
        Models.Date day = new Date();
        day.Today();
        ViewData["Day"] = day.ToString();
        ViewData["UserName"]=HttpContext.Request.Form["UserName"];
        
        return View(@"Person/HomePage");
    }
}
