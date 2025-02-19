using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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

            return RedirectToAction("Index", "NewsArticles");
        }
        // GET: /Admin/CreateAccount
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Admin/CreateAccount
        [HttpPost]
        public async Task<IActionResult> Register(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(account);
                return RedirectToAction(nameof(Login));
            }
            return View(account);
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
