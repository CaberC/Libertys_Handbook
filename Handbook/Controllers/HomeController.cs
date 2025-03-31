using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

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
        return View(@"Index");
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
                ViewData["UserID"]=ID;
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
            ViewData["UserID"]=ID;
            return View(@"Person/HomePage");
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            return View(@"Person/Member"); 
        }
    }
    [HttpPost]
    public IActionResult NewUser(){
        return View(@"Person/NewMember");
    }
    [HttpPost]
    public IActionResult MemberCreate(){
        string UserName;
        string Email;
        string Password;
        int ID;
        try{
            if(HttpContext.Request.Form["UserName"].IsNullOrEmpty() || HttpContext.Request.Form["Email"].IsNullOrEmpty() || HttpContext.Request.Form["Password"].IsNullOrEmpty()){
                throw new Exception("FORM CANNOT BE BLANK");
            }else{
                UserName = HttpContext.Request.Form["UserName"];
                Email = HttpContext.Request.Form["Email"];
                Password = HttpContext.Request.Form["Password"];
            }
            if(!int.TryParse(HttpContext.Request.Form["Zip"],out int Zip)){
                throw new Exception("ZIP NOT AN INTEGER");
            }
            if(!float.TryParse(HttpContext.Request.Form["Range"],out float Range)){
                throw new Exception("RANGE NOT A FLOAT");
            }
            if (!PersonController.CreateUser(out ID, UserName, Email, Password, Zip, Range)){
                throw new Exception("USERCREATE FAILED");
            }
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            return View(@"Person/Member"); 
        }
        ViewData["Title"] = "HomePage";
        Models.Date day = new Date();
        day.Today();
        ViewData["Day"] = day.ToString();
        ViewData["UserName"]= UserName;
        ViewData["UserID"]= ID;
        return View(@"Person/HomePage");
    }
    //public IActionResult MemberLogout(){
    //    _cache.Set("UserID", -1);
    //    return Member();
    //}
    public IActionResult MemberDelete(){
        try{
            int ID = -1;
            if(!HttpContext.Request.Form["UserID"].IsNullOrEmpty()){
                string str = HttpContext.Request.Form["UserID"].ToString();
                if (int.TryParse(str, out ID)){
                    if(ID != -1){
                        PersonController.DeleteUser(ID);
                        ViewData["Body"]= "Beep Boop Deleted";
                        return View(@"Person/Member"); 
                    }
                }
            }
            throw new Exception("Error with UserID");
        }catch (System.Exception e)
        {
            ViewData["Body"]= "Error with Deletion :"+e;
            return View(@"Person/Member");
        }
    }
    public IActionResult ResourceBatch(){
        ViewBag.resource  = ResourceController.GetResources(0,5);
        ViewBag.loadBool = true;
        return View(@"Database");

    }

    [HttpPost]
    public IActionResult ResourcePage(){
        try{
            if(!HttpContext.Request.Form["ResourceID"].IsNullOrEmpty()){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                Resource res = ResourceController.Read((string)HttpContext.Request.Form["ResourceID"]);
                if(res != null){return View(@"Resource", res);}
                else{throw new Exception();}
            }else{throw new Exception();}
        }
        catch (System.Exception)
        {
            return View(@"404");
        }
        
        

    }
}
