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
using DAL.Interfaces;
using DAL.Repositories;
using BLL.Utils;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly); // Or Assembly containing your profiles
            });
            var mapper = config.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddRazorPages();

            builder.Services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Initialize TokenService with configuration
            TokenService.Initialize(builder.Configuration);

            // Cookie authentication
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/Account/Login";
            //        options.LogoutPath = "/Account/Logout";
            //        options.AccessDeniedPath = "/Account/AccessDenied";
            //    });

            // JWT token authentication for API endpoints (optional)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = builder.Configuration["Jwt:Secret"]; // Get JWT Secret Key
                options.TokenValidationParameters.RoleClaimType = "role";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero // Reduce default clock skew for testing
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Lecturer", policy => policy.RequireRole("1"));
                options.AddPolicy("Staff", policy => policy.RequireRole("2"));
                options.AddPolicy("Admin", policy => policy.RequireRole("3"));
            });

            // Register services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<INewsTagService, NewsTagService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<UserUtils>();
            builder.Services.AddHttpContextAccessor();

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;  // Ensure that the cookie is accessible via HTTP only (increased security)
                options.Cookie.IsEssential = true;  // Make the cookie essential for the application
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Use session before authentication
            app.UseSession();  // Make sure session middleware is before authentication

            // Middleware to add the JWT token to the request header (for APIs)
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["JwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseAuthentication(); // Use authentication after session
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
