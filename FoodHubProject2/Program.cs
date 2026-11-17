using FoodHubProject2.Models;
using FoodHubProject2.Repositories;
using FoodHubProject2.Services;
namespace FoodHubProject2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Repository registrations

            builder.Services.AddSingleton(
                typeof(IRepository<>),
                typeof(InMemoryRepository<>));
            var app = builder.Build();
            builder.Services.AddTransient<RestaurantService>();

            builder.Services.AddTransient<OrderService>();

            builder.Services.AddTransient<UserService>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
