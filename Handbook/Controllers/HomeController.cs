using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Handbook.Controllers;

public class HomeController : Controller
{
    //private readonly IMemoryCache _cache;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IMemoryCache cache, ILogger<HomeController> logger)
    {
        _logger = logger;
        //_cache = cache;
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
        try{
            //_cache.TryGetValue("UserID", out int UserID);
            //if (UserID>=0){return MemberLogin(UserID);}else{
            //    Console.WriteLine("UserID "+UserID);
                throw new Exception();//}
        }catch{      
        ViewData["Title"] = "Member Page";
        Models.Date day = new Date();
        day.Today();
        ViewData["Day"] = day.ToString();
        ViewData["Body"]=PersonController.getUsers();
        
        return View(@"Person/Member");
        }
    }
    [HttpPost]
    public IActionResult MemberLogin()
    {
        try {
            Console.WriteLine("New User");
            string UserName = HttpContext.Request.Form["UserName"];
            string Email = HttpContext.Request.Form["Email"];
            string Password = HttpContext.Request.Form["Password"];
            if (UserName != null && Email != null && Password != null){
                if(PersonController.Login(UserName, Email, Password, out int ID)){
                    ViewData["Title"] = "HomePage";
                    Models.Date day = new Date();
                    day.Today();
                    ViewData["Day"] = day.ToString();
                    ViewData["UserName"]= UserName;
                    //_cache.Set("UserID", ID);
                    Console.WriteLine("UserID "+ID);
                }else{throw new Exception("invalid password");}
                return View(@"Person/HomePage");
            }else{
                throw new Exception("input login data");
            }
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            return View(@"Person/Member"); 
        }
    }
    public IActionResult MemberLogin(int ID){
        try{
            Console.WriteLine("Returning User");
            ViewData["Title"] = "HomePage";
            Models.Date day = new Date();
            day.Today();
            ViewData["Day"] = day.ToString();
            ViewData["UserName"]= PersonController.GetUserName(ID);
            return View(@"Person/HomePage");
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            return View(@"Person/Member"); 
        }
    }
    [HttpPost]
    public IActionResult CreateUser(){

    }
    //public IActionResult MemberLogout(){
    //    _cache.Set("UserId", -1);
    //    return Member();
    //}
}
