using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;

        }

        // GET: /Admin/ManageAccounts
        public async Task<IActionResult> ManageAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        public async Task<IActionResult> DetailsAccount(int id) 
        { 
        
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
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
                TempData["Message"] = "Account created successfully.";
                return RedirectToAction(nameof(ManageAccounts));
            }
            TempData["Error"] = "Failed to create account.";
            return View(account);
        }

        // GET: /Admin/EditAccount/{id}
        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        // POST: /Admin/EditAccount/{id}
        [HttpPost]
        public async Task<IActionResult> EditAccount(int id, SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.UpdateAccountAsync(id, account);
                TempData["Message"] = "Account updated successfully.";
                return RedirectToAction(nameof(ManageAccounts));
            }
            TempData["Error"] = "Failed to update account.";
            return View(account);
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }



        // POST: /Admin/DeleteAccount/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            TempData["Message"] = "Account deleted successfully.";
            return RedirectToAction(nameof(ManageAccounts));
        }

        // GET: /Admin/Report
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            var reportData = await _unitOfWork.NewsArticles.GetAllByListAsync(n => n.CreatedDate >= startDate &&
                                                                                    n.CreatedDate <= endDate);
            return View(reportData);
            
        }

        // POST: /Admin/GenerateReport
        [HttpPost]
        public async Task<IActionResult> GenerateReport()
        {
            // Implement your report creation logic using the DAL and BLL services.
          /*  var reportData = await _unitOfWork.NewsArticles.GetByConditionAsync(n => n.CreatedDate >= startDate && 
                                                                                     n.CreatedDate <= endDate); // Replace with actual data*/
            return View();
        }

        // GET: /Admin/SearchAccount
        public async Task<IActionResult> SearchAccount(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["Error"] = "Search term cannot be empty.";
                return RedirectToAction(nameof(ManageAccounts));
            }

            var accounts = await _accountService.GetAllAccountsAsync();
            var filteredAccounts = accounts.Where(a => a.AccountName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                       a.AccountEmail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filteredAccounts.Any())
            {
                TempData["Error"] = "No accounts found matching the search term.";
                return RedirectToAction(nameof(ManageAccounts));
            }

            return View("ManageAccounts", filteredAccounts);
        }
    }
}
