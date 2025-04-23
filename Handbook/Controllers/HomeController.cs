using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Handbook.Controllers;

public class HomeController : Controller
{
    //private readonly IMemoryCache _cache;

    Models.Date day = new Date();
    public IActionResult Index(){return View(@"Index");}
    public IActionResult Privacy(){return View();}
    public IActionResult FAQ(){return View();}
    [HttpPost]
    public IActionResult Translation(){
        ViewBag.loadBool = false;
        ViewData["UserID"] = HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View();
        }
    [HttpGet]
    public IActionResult Database(){
        ViewBag.loadBool = false;
        ViewData["UserID"] = HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View();
    }
    [HttpGet]
    public IActionResult Member(){
        ViewData["Title"] = "Member Page";
        ViewData["Day"] = day.ToString();
        ViewData["Body"]="Welcome to Liberty\'s Handbook";
        //Console.WriteLine(HttpContext.Session.GetInt32("UserID")+", "+HttpContext.Session.GetString("UserName")+" connected at "+day.ToString());
        ViewData["UserID"]=HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Person/Member");
    }

    [HttpPost]
    public IActionResult MemberLogin()
    {
        try {
            //Console.WriteLine("New User");
            string UserName = HttpContext.Request.Form["UserName"];
            string Email = HttpContext.Request.Form["Email"];
            string Password = HttpContext.Request.Form["Password"];
            if (UserName != null && Email != null && Password != null){
                if(PersonController.Login(UserName, Email, Password, out int ID)){
                    ViewData["Title"] = "HomePage";
                    ViewData["Day"] = day.ToString();
                    HttpContext.Session.SetInt32("UserID", ID);
                    HttpContext.Session.SetString("UserName", UserName);
        //Console.WriteLine(HttpContext.Session.GetInt32("UserID")+", "+HttpContext.Session.GetString("UserName")+" connected at "+day.ToString());
                    ViewData["UserID"]=ID;
                    ViewData["UserName"]= UserName;
                    ViewBag.loadBool = false;
                    return View(@"Person/HomePage");
                    //_cache.Set("UserID", ID);
                    //Console.WriteLine("UserID "+ID);
                }else{return View(@"404");}
            }else{
                throw new Exception("input login data");
            }
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            ViewData["Title"] = "Member Page";
            return View(@"Person/Member"); 
        }
    }
    [HttpPost]
    public IActionResult ReLogin(){
        try{            
            //Console.WriteLine("Returning User");
            ViewData["Title"] = "HomePage";
            if(HttpContext.Session.GetInt32("UserID") == null){
                throw new Exception("ID invalid.");
            }
            ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
            ViewData["UserName"]=HttpContext.Session.GetString("UserName");
            ViewBag.loadBool = false;            
            return View(@"Person/HomePage");
        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            ViewData["Title"] = "Member Page";
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

            bool saved = PersonController.CreateUser(out ID, UserName, Email, Password, Zip, Range);
            if (!saved){throw new Exception("USERCREATE FAILED");}

            //Console.WriteLine("Returning User");
            ViewData["Title"] = "HomePage";
            Models.Date day = new Date();
            day.Today();
            ViewData["Day"] = day.ToString();
            HttpContext.Session.SetInt32("UserID", ID);
            HttpContext.Session.SetString("UserName", UserName);
            //Console.WriteLine(HttpContext.Session.GetInt32("UserID")+", "+HttpContext.Session.GetString("UserName")+" connected at "+day.ToString());   
            ViewData["UserID"]=ID;
            ViewData["UserName"]= UserName;
            ViewBag.loadBool = false;
            return View(@"Person/HomePage");

        }catch(Exception e){
            ViewData["Body"]= "invalid login : "+e.ToString();
            ViewData["Title"] = "Member Page";
            return View(@"Person/Member"); 
        }
    }
    public IActionResult LogOut(){
        HttpContext.Session.Set("UserID", null);
        HttpContext.Session.Set("UserName", null);
        return View(@"Index");
    }
    public IActionResult MemberDelete(){
        try{
            int ID = -1;
            if(!HttpContext.Request.Form["UserID"].IsNullOrEmpty()){
                string str = HttpContext.Request.Form["UserID"].ToString();
                if (int.TryParse(str, out ID)){
                    if(ID != -1){
                        PersonController.DeleteUser(ID);
                        ViewData["Body"]= "Beep Boop Deleted";
                        HttpContext.Session.Set("UserID", null);
                        HttpContext.Session.Set("UserName", null);
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
        ViewBag.resource  = ResourceController.GetResources();
        ViewBag.loadBool = true;
        ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Database");
    }

    [HttpGet]
    public IActionResult ResourcePage(string ResourceID){
        try{
            if(ResourceID != null){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                Resource res = ResourceController.Read(ResourceID);
                ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
                ViewData["UserName"]=HttpContext.Session.GetString("UserName");
                if(res != null){return View(@"Resource", res);}
                else{throw new Exception();}
            }else{throw new Exception();}
        }
        catch (System.Exception)
        {
            return View(@"404");
        }
    }
    [HttpPost]
    public IActionResult EditResource(){
        try{
             if(!HttpContext.Request.Form["ResourceID"].IsNullOrEmpty()){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                Resource res = ResourceController.Read((string)HttpContext.Request.Form["ResourceID"]);
                if(res != null){return View(@"EditResource", res);}
                else{throw new Exception();}
            }else{throw new Exception();}
        }
        catch (System.Exception)
        {
            return View(@"404");
        }
    }
    [HttpPost]
    public IActionResult SaveResource(){
        try{
            var ResourceID = HttpContext.Request.Form["ResourceID"];
             if(!ResourceID.IsNullOrEmpty()&&HttpContext.Session.GetInt32("UserID")!=null){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                if (ResourceController.Save((string)ResourceID, (int)HttpContext.Session.GetInt32("UserID"))){
                    return ResourceBatch();
                }else{return View(@"404");}
            }else{throw new Exception();}
        }
        catch (System.Exception)
        {
            return View(@"404");
        }
    }
    [HttpPost]
    public IActionResult LoadSaved(){
        ViewBag.resource  = ResourceController.GetSavedResources( (int) HttpContext.Session.GetInt32("UserID"));
        ViewBag.loadBool = true;
        ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Person/HomePage");
    }
    [HttpPost]
    public IActionResult RemoveSaved(){
        var ResourceID = HttpContext.Request.Form["ResourceID"];
        if(!ResourceID.IsNullOrEmpty()&&HttpContext.Session.GetInt32("UserID")!=null){
            //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
            
            if (int.TryParse(ResourceID, out int resID) && ResourceController.DeleteSaved((int)HttpContext.Session.GetInt32("UserID"), resID)){
            }else{return View(@"404");}
        }
        return LoadSaved();
    }
    [HttpPost]
    public IActionResult UpdateResource(){
        try{
            if(!HttpContext.Request.Form["ResourceID"].IsNullOrEmpty()){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                string[] Args = new string[4];
                Args[0] = HttpContext.Request.Form["ResourceID"];
                Args[1] = HttpContext.Request.Form["Category"];
                Args[2] = HttpContext.Request.Form["Provider"];
                Args[3] = HttpContext.Request.Form["Details"];
                Resource res = ResourceController.Update(Args);
                if(res==null){throw new Exception("Update Failed");}
                ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
                ViewData["UserName"]=HttpContext.Session.GetString("UserName");
                return View(@"Resource", res);
            }else{throw new Exception();}
        }
        catch (System.Exception e)
        {
            ViewData["Exception"] = e.Message;
            return View(@"404");
        }
    }
    [HttpPost]
    public IActionResult CreateResource(){
        try{
            if(!HttpContext.Request.Form["Title"].IsNullOrEmpty() && !HttpContext.Request.Form["Zip"].IsNullOrEmpty()){
                //Console.WriteLine(HttpContext.Request.Form["ResourceID"]);
                string[] Args = new string[5];
                Args[0] = HttpContext.Request.Form["Title"];
                Args[1] = HttpContext.Request.Form["Category"];
                Args[2] = HttpContext.Request.Form["Zip"];
                Args[3] = HttpContext.Request.Form["Provider"];
                Args[4] = HttpContext.Request.Form["Details"];
                Resource res = ResourceController.Create(Args);
                if(res==null){throw new Exception("Create Failed");}
                ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
                ViewData["UserName"]=HttpContext.Session.GetString("UserName");
                return View(@"Resource", res);
            }else{throw new Exception("Forms must be populated");}

        }
        catch (System.Exception e)
        {
            ViewData["Exception"] = e.Message;
            return View(@"404");
        }
    }
    public IActionResult toCreateResource(){
        ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"CreateResources");
    }
    [HttpPost]
    public IActionResult ResourceSearch(){
        ViewBag.resource = ResourceController.ResourceSearch(HttpContext.Request.Form["KeyWord"].ToString(), HttpContext.Request.Form["Zip"].ToString(), HttpContext.Request.Form["Category"].ToString());
        ViewBag.loadBool = true;
        ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Database"); 
    }
    public IActionResult UploadFile(IFormFile pdfFile, string source, string target){
        try{
            if (HttpContext.Session.GetInt32("UserID") != null){
                int UserID = (int)HttpContext.Session.GetInt32("UserID");
                TranslationController.UploadFile(pdfFile, UserID, source, target);
                ViewData["errmessage"] = "success";
                ViewBag.loadBool = false;
                ViewData["UserID"]= UserID;
                ViewData["UserName"]=HttpContext.Session.GetString("UserName");
                return View(@"Translation");
            }else{
                throw new Exception("must be login to upload files");
            }
            
        }catch(Exception e){
            ViewData["errmessage"] = e.Message;
            ViewBag.loadBool = false;
            ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
            ViewData["UserName"]=HttpContext.Session.GetString("UserName");
            return View(@"Translation");
        }
    }
    [HttpPost]
    public IActionResult LoadDocs(){
        if (HttpContext.Session.GetInt32("UserID") == null){
            ViewData["errmessage"] = "must login";
            ViewBag.loadBool = false;
            ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
            ViewData["UserName"]=HttpContext.Session.GetString("UserName");
            return View(@"Translation");
        }
        int UserID = (int)HttpContext.Session.GetInt32("UserID");
        ViewBag.documents = TranslationController.LoadDocs(UserID);
        ViewBag.loadBool = true;
        ViewData["UserID"]= UserID;
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Translation");
    }
    public IActionResult RemoveDocs(){
        if(!HttpContext.Request.Form["DocID"].IsNullOrEmpty() && HttpContext.Session.GetInt32("UserID")!=null){
            int UserID = (int)HttpContext.Session.GetInt32("UserID");
            bool isGone = TranslationController.Remove(UserID, HttpContext.Request.Form["DocID"]);
            if(isGone){
                ViewData["errmessage"] = "success";
                ViewBag.documents = TranslationController.LoadDocs(UserID);
                ViewBag.loadBool = true;
            }else{
                ViewData["errmessage"] = "fail";
                ViewBag.loadBool = false;
            }
        }else{
            ViewData["errmessage"] = "invalid DocID";
            ViewBag.loadBool = false;
        }
        ViewData["UserID"]= HttpContext.Session.GetInt32("UserID");
        ViewData["UserName"]=HttpContext.Session.GetString("UserName");
        return View(@"Translation");
    }
    public ActionResult DownloadFile(string PathID){
        if(int.TryParse(PathID, out int ID)){
            string path = TranslationController.Download(ID,(int)HttpContext.Session.GetInt32("UserID"));
            string current = Directory.GetCurrentDirectory();
            var uploadsFolder = System.IO.Path.Combine(current, "data");
            var filePath = System.IO.Path.Combine(uploadsFolder, path);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "downloaded-file.pdf";
            return File(fileBytes, "application/pdf", fileName);
        }else{
            return null;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

