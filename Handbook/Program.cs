using Handbook.Controllers;
using Handbook.Models;
using Microsoft.Extensions.FileProviders;

namespace Handbook{
    class Program{
        static void Main(){
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
        }

        static void App(){
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            
            app.Run();
        }

       static void Sql(){
            /*
            Resource res = new Resource("The HRDC", 2, 59715, "The HRDC", "Welcome to HRDC, where you’ll find us working to improve our neighbors’ lives by building a better community in Southwest Montana. We invite you to become part of our HRDC family whether you need help or are able to provide help. https://thehrdc.org/");
            if (res.Create()){
                Console.WriteLine("tada");
            }
            var id = "%"+"a"+"%";
            conn.addParam("@Title", System.Data.SqlDbType.VarChar, id);
            List<string[]> strUser = conn.UseParam("SELECT * FROM resource WHERE Title LIKE @Title ORDER BY Zip OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;");

                    <form method="post" style="float:left;">
                        <input type="hidden" id="ResourceID" name="ResourceID" value=@strings[0]/>
                        <input type="image" src="imgs\tsb_new_logo_registered_tm.png" alt="@strings[1]" asp-controller="Home" asp-action="ResourcePage">
                    </form>
            
            */

            var conn = new Models.Start();
            List<string[]> strUser = conn.UseConn("SELECT * FROM resource;");
            
            foreach(string[] row in strUser){
                foreach(string str in row){
                    Console.Write(str+"_");
                }
                Console.WriteLine();
            }
            
        }
    }
}