using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models.Authentication;
using WebFrontToBack.Services;

namespace WebFrontToBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(opt =>  //it is for adding the session to the project
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(10); //The time for active session
            });
            
            //Identity Start
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders(); //For using Identity
            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.AllowedForNewUsers = true;
                //opt.User.AllowedUserNameCharacters = "qhiwjkdf2qw";
            });
            //Identity End

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
            });
            builder.Services.AddScoped<LayoutService>();
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("Home/Error");
            }
            app.UseSession(); //It is for using the session
            app.UseStaticFiles(); //Bununla biz wwwroot daki fayllardan istifade ede bilerik.
            app.UseRouting();   
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name:"areas",
                    pattern:"{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );
            });
            app.Run();
        }
    }
}