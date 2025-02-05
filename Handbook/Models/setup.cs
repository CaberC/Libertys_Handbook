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
        SqlConnection connection = new SqlConnection("Server=localhost\\MSSQLSERVER01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
        public async void BuildConn(){
            Console.WriteLine("START");  
                try{
                    Console.WriteLine("\nQuery data example:");
                    var sql = "SELECT * FROM person;";
                    Console.WriteLine(sql);

                    connection.Open();

                    var command = new SqlCommand(sql, connection);
                    var reader = command.ExecuteReader();

                }
                catch (Exception e)            {
                    Console.WriteLine(e.ToString());
                }
        }
        public Boolean CloseConn(){
            try{
                connection.Close();
                return true;
            }catch(Exception e){
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public String ReadReader(SqlDataReader reader){
            try{
                String str = "";
                Console.WriteLine(reader);
                while (reader.Read()){
                    for (int i  = 0; i< reader.FieldCount; i++){
                        str=str+reader.GetName(i)+" : "+reader[i]+", ";
                    }
                    str=str+"\n";
                    Console.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
                }

                reader.Close();
                return str;
            }catch (Exception e){
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}
