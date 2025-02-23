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
using BLL.DTOs;

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
                new(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new(ClaimTypes.Name, user.AccountName),
                new(ClaimTypes.Role, user.AccountRole.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            HttpContext.User = principal;


            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Redirect based on the user's role
            return role switch
            {
                "3" => RedirectToAction("ManageAccounts", "Admin"),
                "1" => RedirectToAction("Index", "Categories"),
                "2" => RedirectToAction("All", "Lecturer"),
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
        public async Task<IActionResult> Register(AccountDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(dto);
                return RedirectToAction(nameof(Login));
            }
            return View(dto);
        }


        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("All", "Guest");
        }

        private async Task<string> ValidateUserAsync(string email, string password)
        {

            return await _accountService.AuthenticateAsync(email, password);
        }
    }
}
