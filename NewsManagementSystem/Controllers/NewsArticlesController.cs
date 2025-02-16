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

namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly INewsTagService _newsTagService;

        public NewsArticlesController(INewsArticleService newsArticleService, ICategoryService categoryService, INewsTagService newsTagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _newsTagService = newsTagService;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            return View(await _newsArticleService.GetAllNewsArticles());
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticle(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName");
            //ViewBag.Tags = await _newsTagService.GetAllTags();
            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticleCreateDTO newsArticle)
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            //GetAllTags

            if (ModelState.IsValid)
            {
                await _newsArticleService.CreateNewsArticle(newsArticle);
                return RedirectToAction(nameof(Index), "NewsArticles");
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            var newsArticle = await _newsArticleService.GetNewsArticle(id);

            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            var model = new NewsArticleUpdateDTO
            {
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                NewsStatus = newsArticle.NewsStatus,
                CategoryId = newsArticle.CategoryId,
                NewsTagIds = newsArticle.NewsTags.Select(t => t.TagId).ToList()
            };

            return View(model);
        }

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NewsArticleUpdateDTO newsArticle)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticle(id, newsArticle);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var newsArticle = await _newsArticleService.GetNewsArticle(id);

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
            await _newsArticleService.DeleteNewsArticle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
