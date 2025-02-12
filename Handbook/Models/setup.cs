/**
sqlcmd -S LAPTOP-1EVATMPF\MSSQLSERVER01 -E
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
        SqlConnection connection;
        public Start(){
            connection = new SqlConnection("Server=localhost\\MSSQLSERVER01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public bool TestConn(){
            Console.WriteLine("START"); 
            Console.WriteLine("\nQuery data example:");
            var sql = "SELECT * FROM person;";
            Console.WriteLine(sql);
            try{
                String str = UseConn(sql);
                Console.WriteLine(str);
                return true;
            }catch{
                return false;
            }
            

                
        }
        public String ReadReader(SqlDataReader reader){
            try{
                String str = "";
                while (reader.Read()){
                    for (int i  = 0; i< reader.FieldCount; i++){
                        str=str+reader.GetName(i)+" : "+reader[i]+", ";
                    }
                    str=str+"\n";
                }
                reader.Close();
                return str;
            }catch (Exception e){
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public String UseConn(String sql){
            try{
                connection.Open();

                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();
                String str = ReadReader(reader);

                connection.Close();
                return str;
            }
            catch (Exception e)            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
