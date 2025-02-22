using BLL.Interfaces;
using BLL.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using DAL.Entities;
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
            Response.Cookies.Delete("JwtToken");
            // Get the JWT token
            var token = await _accountService.AuthenticateAsync(email, password);
            if (token == null)
            {
                TempData["Error"] = "Invalid email or password.";
                return RedirectToAction("Login", "Account");
            }

            // Define cookie options for the new token
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true
            };

            // Retrieve user account details after authentication
            var user = await _accountService.GetAccountByIdAsync(_userUtils.GetUserFromInputToken(token));

            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.AccountName),
                new Claim(ClaimTypes.Role, user.AccountRole.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            // Store the new token in a cookie
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,  // Only accessible by the server-side (for security)
                Secure = true,    // Only send the cookie over HTTPS (for security)
                SameSite = SameSiteMode.Strict,  // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1)  // Set expiration time for the new token
            });

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
            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Guest", "All");
        }

        private async Task<string> ValidateUserAsync(string email, string password)
        {

            return await _accountService.AuthenticateAsync(email, password);
        }
    }
}
