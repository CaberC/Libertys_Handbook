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
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace setup{
    public static void main(String[] args){
        start.testconn();
    }
    class start{
        static async testconn(){
            Console.WriteLine("START");
            var builder = new SqlConnectionStringBuilder{
                DataSource = "localhost",
                UserID = "root",
                Password = "handbook",
                InitialCatalog = "handbook"
            };

            var connectionString = builder.ConnectionString;

            try{
                await using var connection = new SqlConnection(connectionString);
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                await connection.OpenAsync();

                var sql = "SELECT * FROM user";
                await using var command = new SqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                }
            }
            catch (SqlException e) when (e.Number == /* specific error number */)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }
    }
}
