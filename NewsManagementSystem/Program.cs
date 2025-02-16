using DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
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
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Lecturer", policy => policy.RequireRole("Lecturer"));
                options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });



            var app = builder.Build();




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
                pattern: "{controller=News}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
