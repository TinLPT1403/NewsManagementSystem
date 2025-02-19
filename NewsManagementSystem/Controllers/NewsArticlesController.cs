using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Entities;
using BLL.Interfaces;
using BLL.DTOs;
using Humanizer;
using System.Security.Claims;
using BLL.Utils;

namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly INewsTagService _newsTagService;
        private readonly ITagService _tagService;
        private readonly UserUtils _userUtils;

        public NewsArticlesController(INewsArticleService newsArticleService, ICategoryService categoryService, INewsTagService newsTagService, ITagService tagService, UserUtils userUtils)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _newsTagService = newsTagService;
            _tagService = tagService;
            _userUtils = userUtils;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            var id = _userUtils.GetUserFromToken();
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName");
            ViewData["TagId"] = new SelectList(await _tagService.GetAllTagsAsync(), "TagId", "TagName");
            return View(await _newsArticleService.GetNewsArticlesByUserIdAsync(id));

        }


        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName");
            // Fetch all tags from the service
            var tags = await _tagService.GetAllTagsAsync();

            // Pass the tags to the view using ViewBag
            ViewBag.Tags = tags;
            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticleCreateDTO newsArticle)
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            //GetAllTags

            if (ModelState.IsValid)
            {
                await _newsArticleService.CreateNewsArticleAsync(newsArticle);
                TempData["Message"] = "Article created successfully.";
                return RedirectToAction(nameof(Index), "NewsArticles");
            }

            TempData["Error"] = "Failed to create article.";
            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            var model = new NewsArticleUpdateDTO
            {
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                NewsStatus = newsArticle.NewsStatus,
                CategoryId = newsArticle.CategoryId,
                NewsTagIds = _newsTagService.GetTagsOfArticleAsync(id).Result.Select(t => t.TagId).ToList()
            };

            var newsTag = model.NewsTagIds;

            ViewBag.NewsTag = newsTag;


            var tags = await _tagService.GetAllTagsAsync();

            // Pass the tags to the view using ViewBag
            ViewBag.Tags = tags;
            return View(model);
        }

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NewsArticleUpdateDTO newsArticle)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticleAsync(id, newsArticle);
                TempData["Message"] = "Article updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Failed to update article.";
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _newsArticleService.DeactiveNewsArticleAsync(id);
            TempData["Message"] = "Article deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: NewsArticles/Search
        public async Task<IActionResult> Search(string searchTerm, int? categoryId, int? tagId)
        {
            var articles = await _newsArticleService.GetActiveNewsArticlesAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                articles = articles.Where(a => a.NewsTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                               a.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue)
            {
                articles = articles.Where(a => a.CategoryId == categoryId.Value).ToList();
            }

            if (tagId.HasValue)
            {
                var taggedArticles = await _newsTagService.GetArticlesFromTagAsync(tagId.Value);
                articles = articles.Intersect(taggedArticles).ToList();
            }

            ViewData["CategoryId"] = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName", categoryId);
            ViewData["TagId"] = new SelectList(await _tagService.GetAllTagsAsync(), "TagId", "TagName", tagId);

            return View("Index", articles);
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
