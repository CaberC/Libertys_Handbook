/**
sqlcmd -S LAPTOP-1EVATMPF\MSSQLSERVER01 -E
**/
using Microsoft.Data.SqlClient;

namespace Handbook.Models{

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
                string[] str = UseConn(sql)[0];
                Console.WriteLine(str);
                return true;
            }catch{
                return false;
            }
                
        }
        public List<string[]> ReadReader(SqlDataReader reader){
            try{
                List<string[]> output = new List<string[]>();
                while (reader.Read()){
                    string[] row = new string[reader.FieldCount];
                    for (int i  = 0; i< reader.FieldCount; i++){
                        string str = "";
                        str = str+reader[i];
                        row[i]=str;
                    }
                    output.Add(row);
                }
                reader.Close();
                return output;
            }catch (Exception e){
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public List<string[]> UseConn(string sql){
            try{
                connection.Open();

                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();
                Console.WriteLine(reader.RecordsAffected);
                List<string[]> table = ReadReader(reader);

                connection.Close();
                return table; 
            }
            catch (Exception e)            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
