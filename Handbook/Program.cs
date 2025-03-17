using Handbook.Models;

namespace Handbook{
    class Program{
        static void Main(){
            Console.WriteLine("Enter program to run [sql or app]:");
            string prog = Console.ReadLine();
            if (prog.Equals("sql")){
                Sql();
            }else if (prog.Equals("app")){
                App();
            }
        }

        static void App(){
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            app.Run();
        }

       static void Sql(){
            var conn = new Models.Start();
            List<string[]> strUser = conn.UseConn("SELECT * FROM resource;");
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
            Resource res = new Resource("Gallatin Valley Food Bank", 1, 59715, "infogvfb@thehrdc.org",
             "Gallatin Valley Food Bank’s (GVFB) mission is to improve food security throughout Southwest Montana.\n"+
             "@https://gallatinvalleyfoodbank.org/about-us/\n");
            res.Create();
            strUser = conn.UseConn("SELECT * FROM resource;");
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
            int newID = res.GetID();
            res.SetCategory(2);
            res.Update(newID);
            strUser = conn.UseConn("SELECT * FROM resource;");
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
            res.Read(newID);
            Console.WriteLine(res.GetCategory());
            res.Delete(newID);
            strUser = conn.UseConn("SELECT * FROM resource;");
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
            //CREATE TABLE resource(
            // ID int NOT NULL,
            // Title varchar(255), 
            // Category int, 
            // Zip int, 
            // Provider varchar(255), 
            // Details varchar(255), 
            // PRIMARY KEY (ID) );
            //"INSERT INTO resource (ID, Title, Category, Zip, Provider, Details) VALUES (0, \'Handling ICE\', 0, 0, \'They See BLue\', \'https://www.theyseeblue.org/community-resources/encountering-ice\');"
        }
    }
}