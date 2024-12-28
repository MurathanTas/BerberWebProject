using BerberWeb.Business.Abstract;
using BerberWeb.Business.Concrete;
using BerberWeb.DataAccess;
using BerberWeb.DataAccess.Abstract;
using BerberWeb.DataAccess.Context;
using BerberWeb.DataAccess.EntityFramework;
using BerberWeb.DataAccess.Repository;
using BerberWeb.Entity.Entities;
using BerberWeb.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BerberWebDbContext>(options => options.UseNpgsql(builder.Configuration
               .GetConnectionString("PostgreSQL")));
            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Þifre gereksinimlerini kaldýrdýk
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            }).AddEntityFrameworkStores<BerberWebDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddScoped<IAboutService, AboutManager>();
            builder.Services.AddScoped<IAboutDal, EfAboutDal>();
            builder.Services.AddScoped<IContactService, ContactManager>();
            builder.Services.AddScoped<IContactDal, EfContactDal>();
            builder.Services.AddScoped<IServiceService, ServiceManager>();
            builder.Services.AddScoped<IServiceDal, EfServiceDal>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Login/SignIn";
                cfg.LogoutPath = "/Login/Logout";
                cfg.AccessDeniedPath = "/Error/AccessDenied";
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Task.Run(() => SeeData.InitializeAsync(services)).Wait();
            }

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
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
