using BLL.Interfaces;
using BLL.Services;
using DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new MapperConfiguration(cfg => {
                cfg.AddMaps(typeof(Program).Assembly); // Or Assembly containing your profiles
            });
            var mapper = config.CreateMapper();
            builder.Services.AddSingleton(mapper);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddRazorPages();

            builder.Services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //JWT token

            var key = Encoding.ASCII.GetBytes("ilovecat");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ilovecat.com",
                    ValidAudience = "ilovecat.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

            });
            //register service
            // Register the DbContext (adjust options as needed)

            // Register IUnitOfWork and its implementation
            builder.Services.AddScoped<DAL.UnitOfWork.IUnitOfWork, DAL.UnitOfWork.UnitOfWork>();

            // Existing registrations
            builder.Services.AddScoped<BLL.Interfaces.IAccountService, BLL.Services.AccountService>();
            builder.Services.AddScoped<BLL.Interfaces.ICategoryService, BLL.Services.CategoryService>();
            builder.Services.AddScoped<BLL.Interfaces.INewsArticleService, BLL.Services.NewsArticleService>();
            builder.Services.AddScoped<BLL.Interfaces.INewsTagService, BLL.Services.NewsTagService>();
            builder.Services.AddScoped<BLL.Interfaces.ITagService, BLL.Services.TagService>();
            builder.Services.AddScoped<DAL.Interfaces.INewsArticleRepository, DAL.Repositories.NewsArticleRepository>();
            builder.Services.AddHttpContextAccessor();
            // Register cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // Specify the paths for login, logout, and access denied as needed.
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Lecturer", policy => policy.RequireRole("1"));
                options.AddPolicy("Staff", policy => policy.RequireRole("2"));
                options.AddPolicy("Admin", policy => policy.RequireRole("3"));
            });


            var app = builder.Build();

            // Register IMapper as a singleton




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
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
                pattern: "{controller=Account}/{action=Login}/{id?}");

            //app.MapRazorPages()

            app.Run();
        }
    }
}
