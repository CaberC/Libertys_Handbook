/**
sqlcmd -S LAPTOP-1EVATMPF\MSSQLSERVER01 -E
**/
using Microsoft.Data.SqlClient;

namespace Program{

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
                string str = UseConn(sql);
                Console.WriteLine(str);
                return true;
            }catch{
                return false;
            }
                
        }
        public string ReadReader(SqlDataReader reader){
            try{
                string str = "";
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
        public string UseConn(string sql){
            try{
                connection.Open();

                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();
                string str = ReadReader(reader);

                connection.Close();
                return str;
            }
            catch (Exception e)            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public static string[] SplitReader(string str){
            string[] sub = str.Split(',');
            string[] outStr = new string[sub.Length*2];

            int i = 0;
            foreach(string s in sub){
                int index = s.IndexOf(" \n");
                if (index>0) s.Remove(index);
                index = s.IndexOf(':')+2;
                string subStr = s.Substring(index);
                outStr[i] = subStr;
                i++;
            }

            return outStr;
        }
    }
}
