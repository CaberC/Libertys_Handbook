namespace Program{
    class Program{
        static void Main(){
            Start conn = new Start();
            conn.TestConn();
            var str = conn.UseConn("SELECT UserName FROM person WHERE ID = 0;");
            Console.WriteLine(str);
            
            Person person = new Person("Caber", "ex@ampl.e", "12345", 11111, 1.5f);
            if (person.Create()) Console.WriteLine("successful create");
            int id = person.GetID();
            person.SetPassword("BetterPass1!");
            person.SetRange(person.GetRange()+1f);
            if (person.Update(id)) Console.WriteLine("successful update");
            if (person.Read(id)) Console.WriteLine("successful read");
            Console.WriteLine(person.GetPassword());
            Console.WriteLine(person.GetRange());
            if (person.Delete(id)) Console.WriteLine("successful Delete");
            if (!person.Read(id)) Console.WriteLine("double check good");

        }

        void App(){
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
    }
}