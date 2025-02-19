using BLL.Interfaces;
using BLL.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserUtils _userUtils;
        public AccountController(IAccountService accountService, UserUtils userUtils)
        {
            _accountService = accountService;
            _userUtils = userUtils;
        }
        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Get the JWT token
            var token = await _accountService.AuthenticateAsync(email, password);
            if (token == null)
            {
                return Unauthorized();
            }
            var user = await _accountService.GetAccountByIdAsync(_userUtils.GetUserFromToken());
            // Store the JWT in a secure HttpOnly cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // Only accessible by the server-side (for security)
                Secure = true,    // Only send the cookie over HTTPS (for security)
                SameSite = SameSiteMode.Strict,  // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1)  // Set expiration time
            };

            // Store token in a cookie
            Response.Cookies.Append("JwtToken", token, cookieOptions);
            var role = user.AccountRole;
            return role switch
            {
                1 => RedirectToAction("ManageAccounts", "Admin"),
                2 => RedirectToAction("Index", "Categories"),
                3 => RedirectToAction("All", "Lecturer"),
                _ => RedirectToAction("Index", "NewsArticles") // Default fallback
            };
        }



        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "News");
        }

        private async Task<string> ValidateUserAsync(string email, string password)
        {

            return await _accountService.AuthenticateAsync(email, password);
        }
    }
}
