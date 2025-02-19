using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        public LecturerController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        // GET: LecturerController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> All()
        {
            return View( await _newsArticleService.GetAllNewsArticlesAsync());
        }

        // GET: LecturerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LecturerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LecturerController/Create
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

        // GET: LecturerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LecturerController/Edit/5
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

        // GET: LecturerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LecturerController/Delete/5
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
