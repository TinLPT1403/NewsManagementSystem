using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;

        public StaffController(ICategoryService categoryService, INewsArticleService newsArticleService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
        }

        // GET: /Staff/ManageCategories
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: /Staff/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: /Staff/CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(category);
        }

        // GET: /Staff/EditCategory/{id}
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Staff/EditCategory/{id}
        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(id, category);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(category);
        }

        // POST: /Staff/DeleteCategory/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction(nameof(ManageCategories));
        }

        // GET: /Staff/ManageNewsArticles
        public async Task<IActionResult> ManageNewsArticles()
        {
            var articles = await _newsArticleService.GetAllNewsArticles();
            return View(articles);
        }

        // GET: /Staff/CreateNewsArticle
        public IActionResult CreateNewsArticle()
        {
            return View();
        }

        // POST: /Staff/CreateNewsArticle
        [HttpPost]
        public async Task<IActionResult> CreateNewsArticle(NewsArticleCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.CreateNewsArticle(dto);
                return RedirectToAction(nameof(ManageNewsArticles));
            }
            return View(dto);
        }

        // GET: /Staff/EditNewsArticle/{id}
        public async Task<IActionResult> EditNewsArticle(string id)
        {
            var article = await _newsArticleService.GetNewsArticle(id);
            if (article == null) return NotFound();
            return View(article);
        }

        // POST: /Staff/EditNewsArticle/{id}
        [HttpPost]
        public async Task<IActionResult> EditNewsArticle(string id, NewsArticleUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticle(id, dto);
                return RedirectToAction(nameof(ManageNewsArticles));
            }
            return View(dto);
        }

        // POST: /Staff/DeleteNewsArticle/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            await _newsArticleService.DeleteNewsArticle(id);
            return RedirectToAction(nameof(ManageNewsArticles));
        }

        // GET: /Staff/MyProfile
        public IActionResult MyProfile()
        {
            // Implement logic to get the current user's profile
            return View();
        }

        // GET: /Staff/MyNewsHistory
        public async Task<IActionResult> MyNewsHistory()
        {
            var userId = GetUserFromToken();
            var newsHistory = await _newsArticleService.GetNewsArticlesByUserId(userId);
            return View(newsHistory);
        }

        private int GetUserFromToken()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
