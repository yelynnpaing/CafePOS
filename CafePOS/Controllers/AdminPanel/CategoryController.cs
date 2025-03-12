using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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

        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await categories.GetAllAsync());
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

        [Route("edit/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return View(await categories.GetByIdAsync(id, new QueryOptions<Category> { Includes = "Items" }));
        }

        [Route("edit/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)        
        {
            if (ModelState.IsValid)
            {
                await categories.UpdateAsync(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Route("delete/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            return View(await categories.GetByIdAsync(id, new QueryOptions<Category> { Includes = "Items"})); 
        }

        [Route("delete/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            await categories.DeleteAsync(category.CategoryId);
            return RedirectToAction("Index");
        }

    }
}
