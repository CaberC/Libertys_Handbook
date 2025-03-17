using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;
class PersonController : Controller{
    public static string getUsers(){
        var conn = new Models.Start();
        List<string[]> strUser = conn.UseConn("SELECT * FROM person;");
        string strOut= "<ul>";
        foreach(string[] str in strUser){
            strOut = strOut+"<li>"+str[1]+"</li>";
        }
        strOut = strOut+"</ul>";
        return strOut;
    }
}
