using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;
class PersonController : Controller{
    public static string getUsers(){
        var conn = new Models.Start();
        List<string[]> strUser = conn.UseConn("SELECT * FROM person;");
        string strOut= "";
        foreach(string[] str in strUser){
            strOut = strOut+str[1]+"_"+str[2]+"_"+str[3];
        }
        strOut = strOut+"\n";
        return strOut;
    }
    public static bool Login(string UserName, string Email, string Password, out int ID){
        Person user = new Person(UserName,Email,"",0,0f);
        int id = user.GetID();
        if(id<0){
            ID = -1;
            return false;
        }
        user.Read(id);
        string pass = user.GetPassword();
        if(!pass.Equals(Password)){
            ID = -1;
            return false;
        }
        ID = id;
        return true;
    }
    public static string GetUserName(int ID){
        Person user = new Person(ID);
        return user.GetUserName();
        
    }

    public static bool CreateUser(string UserName, string Email, string Password, int Zip, float Range){
        try{
            Person user = new Person(UserName, Email, Password, Zip, Range);
            if(!user.Create()){
                throw new Exception("Create Failed");
            }else{
                return true;
            }
        }catch(Exception e){
            Console.WriteLine(e);
            return false;
        }
        
    }
}
