using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
// used https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-9.0
namespace Handbook.Controllers;
class TranslationController : Controller{
    [HttpPost]
    public static void UploadFile(IFormFile pdfFile, int PersonID){
        if (pdfFile == null || pdfFile.Length <= 0){
            throw new Exception("select a PDF file to upload");
        }
        if(pdfFile.Length > 20000000){
            throw new Exception("PDF is too large"); 
        }
        string oldName = pdfFile.FileName;
        string newName = System.IO.Path.GetRandomFileName()+"_"+DateTime.Today.Year+DateTime.Today.Month+DateTime.Today.Day;
        var fileExtension = System.IO.Path.GetExtension(oldName);
        if (fileExtension.ToLower() != ".pdf"){
            throw new Exception("only PDF please");
        }
        Models.Path p = new Models.Path(oldName, newName, PersonID);
        string current = Directory.GetCurrentDirectory();
        var uploadsFolder = System.IO.Path.Combine(current, "data");
        var filePath = System.IO.Path.Combine(uploadsFolder, newName);
        using (var stream = new FileStream(filePath, FileMode.Create)){
            pdfFile.CopyTo(stream);
        }
        p.Create();
    }
    public static List<string[]> LoadDocs(int PersonID){
        List<string[]> docs = Models.Path.GetDocs(PersonID);
        return docs;
    }
    public static bool Remove(int PersonID, string DocID){
        try{
            Models.Path p = new Models.Path("","",PersonID);
            if(int.TryParse(DocID, out int id)){
                p.Read(id);
                //delete from server
                return p.Delete(id);
            }else{
                return false;
            }
        }
        catch (System.Exception e){
            Console.WriteLine(e.Message);
            return false;
        }
        
    }
}
