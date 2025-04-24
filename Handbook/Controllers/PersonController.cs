using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers;
class PersonController{
    public static string getUsers(){
        var conn = new Models.Start();
        List<string[]> strUser = conn.UseConn("SELECT * FROM person;");
        string strOut= "";
        foreach(string[] str in strUser){
            strOut = strOut+str[1]+"_"+str[2]+"_"+str[3];
            strOut = strOut+"\n";
        }
        return strOut;
    }
    public static bool Login(string UserName, string Email, string Password, out int ID){
        Person user = new Person(UserName,Email,"",0,0f);
        int id = user.GetID();
        ID = id;
        if(id<0){
            return false;
        }
        user.Read(id);
        if(!user.CheckPassword(Password)){
            return false;
        }
        return true;
    }
    public static bool CreateUser(out int ID, string UserName, string Email, string Password, int Zip, float Range){
        try{
            Person user = new Person(UserName, Email, Password, Zip, Range);
            if(!user.Create()){
                throw new Exception("Create Failed");
            }else{
                Login(UserName, Email, Password, out ID);
                return true;
            }
        }catch(Exception e){
            Console.WriteLine(e);
            ID = -1;
            return false;
        }
        
    }
    public static bool DeleteUser(int ID){
        try{ 
            Person user = new Person(ID);
            if(!user.Delete(ID)){
                throw new Exception();
            }
            return true;
        }catch (System.Exception){
            Console.WriteLine("Could not Delete");
            return false;
        }
    }
    /*
    public static void cacheLogin(){
        IMemoryCache memoryCache;
        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(3));
        memoryCache.Set("ID", GetID(), options);
        Console.WriteLine("Set");
    }
    */
}
