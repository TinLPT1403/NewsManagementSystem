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
        public IActionResult Login()
        {
            // Force clear the authentication token
            Response.Cookies.Delete("JwtToken");

            // Clear any authentication context
            HttpContext.SignOutAsync();
            // Add cache control headers to prevent browser caching
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Response.Cookies.Delete("JwtToken");
            // Get the JWT token
            var token = await _accountService.AuthenticateAsync(email, password);
            if (token == null)
            {
                return Unauthorized();
            }

            // Define cookie options for the new token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // Only accessible by the server-side (for security)
                Secure = true,    // Only send the cookie over HTTPS (for security)
                SameSite = SameSiteMode.Strict,  // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1)  // Set expiration time for the new token
            };

            // Retrieve user account details after authentication
            var user = await _accountService.GetAccountByIdAsync(_userUtils.GetUserFromInputToken(token));



            // Store the new token in a cookie
            Response.Cookies.Append("JwtToken", token, cookieOptions);

            // Use the role from the user object directly instead of fetching it from the old token
            int role = user.AccountRole;
            Console.WriteLine("ROLE IS :" + role);

            // Redirect based on the user's role
            return role switch
            {
                3 => RedirectToAction("ManageAccounts", "Admin"),
                1 => RedirectToAction("Index", "Categories"),
                2 => RedirectToAction("All", "Lecturer"),
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
