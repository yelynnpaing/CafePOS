using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private Repository<Category> categories;
        public CategoryController(AppDbContext context)
        {
            categories = new Repository<Category>(context);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId, CatName")] Category category)
        {
            if (ModelState.IsValid)
            {
                await categories.AddAsync(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }


    }
}
