using Handbook.Controllers;
using Handbook.Models;
using Microsoft.Extensions.FileProviders;

namespace Handbook{
    class Program{
        static void Main(){
            /*
            Console.WriteLine("Enter program to run [sql or app]:");
            string prog = Console.ReadLine();
            if (prog.Equals("sql")){
                Sql();
            }else if (prog.Equals("app")){
                App();
            }else{
                Console.WriteLine("Try Again");
                Main();
            }
            */
            App();
        }

        static void App(){
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Errorot");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.UseStaticFiles(new StaticFileOptions {FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", "imgs")), RequestPath = "/imgs"});

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            
            app.Run();
        }
        /*
       static void Sql(){
            var conn = new Models.Start();
            
            conn.addParam("@column", System.Data.SqlDbType.VarChar, "Title");
            conn.addParam("@key", System.Data.SqlDbType.VarChar, "E");
            
            List<string[]> strUser = conn.UseConn("SELECT * FROM resource WHERE Title LIKE '%ICE%';");
            
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
       }*/
    }
}