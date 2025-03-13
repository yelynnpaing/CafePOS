using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{
    [Route("admin/item")]
    public class ItemController : Controller
    {
        private Repository<Item> items;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            items = new Repository<Item>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Items()
        {
            return View(await items.GetAllAsync());
        }

        [Route("addEdit")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(Guid id)
        {
            ViewBag.Categories = await categories.GetAllAsync();
            if(id == Guid.Empty)
            {
                ViewBag.Operation = "Item Add";
                return View(new Item());
            }
            else
            {
                Item item = await items.GetByIdAsync(id, new QueryOptions<Item>
                {
                    Includes = "Category"
                });
                ViewBag.Operation = "Item Edit";
                return View(item);
            }           
        }

        [Route("addEdit")]
        [HttpPost]
        public async Task<IActionResult> AddEdit(Item item, Guid CatId)
        {
            ViewBag.Categories = await categories.GetAllAsync();
            if (ModelState.IsValid)
            {
                if(item.ImageFile != null)
                {
                    string imgaeUploader = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + item.ImageFile.FileName;
                    string filePath = Path.Combine(imgaeUploader, uniqueFileName);
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.ImageFile.CopyToAsync(fileStream);
                    }
                    item.ImageUrl = uniqueFileName;
                    if(item.ItemId == Guid.Empty)
                    {
                        item.CategoryId = CatId;
                        item.IsAvailable = true;
                        await items.AddAsync(item);
                        return RedirectToAction("Index", "Item");
                    }
                }
            }
            return RedirectToAction("Index", "Item");
        }
    }
}
