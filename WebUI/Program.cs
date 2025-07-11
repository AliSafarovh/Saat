using Application;
using Application.Repositories.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Contexts;
using Persistence.Repositories.Orders;

namespace Saat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ?? Connection string (API il? eyni olmalýdýr)
            builder.Services.AddDbContext<SaatDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ?? Application v? Persistence servisl?rini qeyd et (eynil? API-d?ki kimi)
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            // ?? HTTP çaðýrýþlar üçün
            builder.Services.AddHttpClient();

            // ??? View d?st?yi
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ??? Exception s?hif?si
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Category}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
