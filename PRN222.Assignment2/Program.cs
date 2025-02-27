using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment1.Business.Profiles;
using PRN222.Assignment1.Business.Services.Implements;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment1.Data.Context;
using PRN222.Assignment1.Data.Repositories.Implements;
using PRN222.Assignment1.Data.Repositories;
using PRN222.Assignment2.Hubs;

namespace PRN222.Assignment2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<Prn222asm1Context>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
            builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
            builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            builder.Services.AddScoped<IAttendeeService, AttendeeService>();

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHub<EventHub>("/eventHub");

            app.MapRazorPages();

            app.Run();
        }
    }
}
