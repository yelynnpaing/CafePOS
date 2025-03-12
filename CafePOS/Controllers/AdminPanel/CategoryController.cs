using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{    
    public class CategoryController : Controller
    {
        private Repository<Category> categories;
        public CategoryController(AppDbContext context)
        {
            categories = new Repository<Category>(context);
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await categories.GetAllAsync());
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
       
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
      
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return View(await categories.GetByIdAsync(id, new QueryOptions<Category> { Includes = "Items" }));
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)        {
            
            if (ModelState.IsValid)
            {
                await categories.UpdateAsync(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }


    }
}
