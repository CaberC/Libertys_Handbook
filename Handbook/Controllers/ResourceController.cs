using Microsoft.AspNetCore.Mvc;
using Handbook.Models;
using Microsoft.IdentityModel.Tokens;

namespace Handbook.Controllers;
class ResourceController : Controller{
    public static List<string[]> GetResources(){
        List<string[]> res = Resource.GetResources();
        return res;
    }
    public static Resource Read(string ID){
        try{
            if (ID.EndsWith('/')){
                ID=ID.TrimEnd('/');
            }
            Resource res = new Resource(ID);
            return res;
        }
        catch (Exception e){
            Console.WriteLine(e);
            return null;
        }
    }
    public static Resource Update(string[] Args){
        try{
            for(int i = 0; i<Args.Length; i++){
                if(Args[i].EndsWith('/')){
                    Args[i]=Args[i].TrimEnd('/');
                }
            }
            Resource res = new Resource(Args[0]);
            int.TryParse(Args[1], out int cat);
            res.SetCategory(cat);
            res.SetProvider(Args[2]);
            res.SetDetails(Args[3]);
            int.TryParse(Args[0], out int ID);
            res.Update(ID);
            return res;
        }
        catch (Exception e){
            Console.WriteLine(e);
            return null;
        }
    }
    public static Resource Create(string[] Args){
        try{
            for(int i = 0; i<Args.Length; i++){
                if(Args[i].EndsWith('/')){
                    Args[i]=Args[i].TrimEnd('/');
                }
            }
            if(!int.TryParse(Args[1], out int Category) || !int.TryParse(Args[2], out int Zip)){
                throw new Exception("Category or Zip: Must be an integer");
            }
            Resource res = new Resource(Args[0], Category, Zip, Args[3], Args[4]);
            if(!res.Create()){
                throw new Exception("Create Failed");
            }
            return res;
        }
        catch (Exception e){
            Console.WriteLine(e);
            return null;
        }
    }
    public static List<string[]> ResourceSearch(string KeyWord, string ZipStr, string Category){
        if(ZipStr.IsNullOrEmpty()){ZipStr="-1";}
        int.TryParse(ZipStr, out int Zip);
        int.TryParse(Category, out int Cat);
        List<string[]> list = Resource.ResourceSearch(KeyWord, Zip, Cat);
         
        return list;
    }
}