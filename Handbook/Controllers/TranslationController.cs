using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Data;
using Google.Cloud.Translate.V3;
using Google.Apis.Auth.OAuth2;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Handbook.Controllers;
class TranslationController : Controller{
    [HttpPost]
    public static void UploadFile(IFormFile pdfFile, int PersonID, string source, string target){
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
        Translate(newName, source, target);
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
                string current = Directory.GetCurrentDirectory();
                var uploadsFolder = System.IO.Path.Combine(current, "data");
                var filePath = System.IO.Path.Combine(uploadsFolder, p.GetPathStr());
                if (System.IO.File.Exists(filePath)){
                try{
                        System.IO.File.Delete(filePath);
                    }catch (Exception e){
                        Console.WriteLine(e.Message);
                    }
                }
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
    public static string Download(int PathID, int PersonID){
        Models.Path p = new Models.Path("","",PersonID);
        p.Read(PathID);
        if(p.GetPersonID()!=PersonID){
            return "";
        }
        return p.GetPathStr();
    }


    private async static void Translate(string fileName, string source, string target){
        
        string parent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        IEnumerable<string> rows = System.IO.File.ReadLines(parent+"\\appsettings.csv");
        var pdf = Convert.ToBase64String(System.IO.File.ReadAllBytes(parent+"/Handbook/data/"+fileName));
        var client = new HttpClient();
        var credential = GoogleCredential
            .FromFile(rows.ElementAt(3))
            .CreateScoped("https://www.googleapis.com/auth/cloud-translation");
        var accessToken = await credential.UnderlyingCredential
            .GetAccessTokenForRequestAsync();
            Console.WriteLine(accessToken);
        var url = "https://translation.googleapis.com/v3/projects/"+rows.ElementAt(1)+"/locations/us-central1:translateDocument";
        //Console.WriteLine(url);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        var requestData = new TranslationRequest{
            source_language_code = source,
            target_language_code = target,
            document_input_config = new DocumentInputConfig{
                mimeType = "application/pdf",
                content = pdf
            },
            document_output_config = new DocumentOutputConfig{
                gcsDestination = new GcsDestination{
                    outputUriPrefix = "gs://"+parent+"/Handbook/data/"
                }
            },
            isTranslateNativePdfOnly = false,
            parent = "projects/"+rows.ElementAt(1)+"/locations/us-central1"
        };

        var json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        if (response.IsSuccessStatusCode){
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseJson);
            //Console.WriteLine("Success");
        }else{
            Console.WriteLine("Error: " + response.StatusCode);
            //Console.WriteLine(response);
        }
    }
    public class DocumentInputConfig{
        public string mimeType { get; set; }
        public string content { get; set; }
    }

    public class GcsDestination{
        public string outputUriPrefix { get; set; }
    }

    public class DocumentOutputConfig{
        public GcsDestination gcsDestination { get; set; }
    }
    public class TranslationRequest{
        public string source_language_code { get; set; }
        public string target_language_code { get; set; }
        public DocumentInputConfig document_input_config { get; set; }
        public DocumentOutputConfig document_output_config { get; set; }
        public bool isTranslateNativePdfOnly { get; set; }
        public string parent { get; set; }
    }

}
