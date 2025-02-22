using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    public class GuestController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        public GuestController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        // GET: GuestController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> All()
        {
            var list = await _newsArticleService.GetActiveNewsArticlesAsync();
            return View(list);
        }

        // GET: GuestController/Details/
        // GET: GuestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GuestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GuestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GuestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GuestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
