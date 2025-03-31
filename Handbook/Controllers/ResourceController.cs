using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;
class ResourceController : Controller{
    public static List<string[]> GetResources(int Page, int Rows){
        return Resource.GetResources(Page*Rows,Rows);
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
}