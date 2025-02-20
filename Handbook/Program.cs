namespace Program{
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
            var conn = new Start();
            var str = conn.UseConn("SELECT * FROM path;");
            Console.WriteLine(str);
            Path pdf = new Path("Constitution",".\\data\\constitution.pdf",0);
            if (pdf.Create()) Console.WriteLine("success");
            str = conn.UseConn("SELECT * FROM path;");
            Console.WriteLine(str);
            
            
        }
    }
}