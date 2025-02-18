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

        // GET: /Admin/CreateAccount
        public IActionResult CreateAccount()
        {
            return View();
        }

        // POST: /Admin/CreateAccount
        [HttpPost]
        public async Task<IActionResult> CreateAccount(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(account);
                return RedirectToAction(nameof(ManageAccounts));
            }
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
                return RedirectToAction(nameof(ManageAccounts));
            }
            return View(account);
        }

        // POST: /Admin/DeleteAccount/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _accountService.DeleteAccountAsync(id);
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
    }
}
