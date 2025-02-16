using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    [AllowAnonymous]
    public class NewsController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        // GET: /News/Index
        // Lists only active news articles.
        public async Task<IActionResult> Index()
        {
            var articles = await _newsArticleService.GetAllNewsArticles();
            var activeArticles = articles.Where(a => a.NewsStatus == true).ToList();
            return View(activeArticles);
        }

        // GET: /News/Details/{id}
        // Shows details if the article exists and is active.
        public async Task<IActionResult> Details(string id)
        {
            var article = await _newsArticleService.GetNewsArticle(id);
            if (article == null || article.NewsStatus != true)
            {
                return NotFound();
            }
            return View(article);
        }
    }
}
