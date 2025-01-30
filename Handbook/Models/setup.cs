/**
{
    "sqltools.connections": [
        {
            "mysqlOptions": {
                "authProtocol": "xprotocol",
                "enableSsl": "Disabled"
            },
            "previewLimit": 50,
            "server": "localhost",
            "port": 33060,
            "driver": "MySQL",
            "name": "handbook",
            "database": "handbook",
            "username": "root",
            "connectionTimeout": 300
        }
    ]
} 
**/
using Microsoft;
using System;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Setup;
using System.Security;

namespace Setup{

    class Start{
        public static async void Testconn(){
            Console.WriteLine("START");
            
            var connectionString = "Server=local, 3360;Database=handbook;";
            SecureString pwd = getPwd();
            
            if(pwd != null){
                SqlCredential cred = null;
                try{
                    cred = new SqlCredential("root", pwd);
                }catch(Exception e){
                    Console.WriteLine(e.ToString());
                }
                
                try{
                    await using var connection = new SqlConnection(connectionString, cred);
                    Console.WriteLine("\nQuery data example:");
                    var sql = "SELECT * FROM user";
                    Console.WriteLine(sql);

                    connection.Open();

                    await using var command = new SqlCommand(sql, connection);
                    await using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    }
                }
                catch (Exception e)            {
                    Console.WriteLine(e.ToString());
                }
            }
            
            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }

        private static SecureString getPwd(){
            SecureString securePwd = new SecureString();
            string file = @"C:\Users\Caber\Documents\_Capstone\Libertys_Handbook\Handbook\data\word.txt"; 
            string str;
            
            if (File.Exists(file)) { 
                str = File.ReadAllText(file);
                char[] chars = str.ToCharArray();
                foreach(char c in chars){
                    securePwd.AppendChar(c);
                }
            }else{
                Console.WriteLine("ERR: no password found");
                securePwd = null;
            }
            securePwd.MakeReadOnly();
            return securePwd;
        }
    }
}
