using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;
class ResourceController : Controller{
    public static List<string[]> GetResources(int Page, int Rows){
        return Resource.GetResources(Page*Rows,Rows);
    }
}