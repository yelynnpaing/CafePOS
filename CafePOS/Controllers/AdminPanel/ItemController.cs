using CafePOS.Data;
using CafePOS.Models;
using CafePOS.ViewModels;
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
                ViewData["ImageUrl"] = item.ImageUrl;
                return View(item);
            }           
        }

        [Route("addEdit")]
        [HttpPost]
        public async Task<IActionResult> AddEdit(Item item, Guid CatId)
        {
            ViewBag.Categories = await categories.GetAllAsync();          

            if (item.ItemId == Guid.Empty)
            {               
                if (item.ImageFile != null)
                {
                    string imageUploader = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + item.ImageFile.FileName;
                    string filePath = Path.Combine(imageUploader, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.ImageFile.CopyToAsync(fileStream);
                    }
                    item.ImageUrl = uniqueFileName;
                    item.CategoryId = CatId;
                    item.IsAvailable = true;
                    item.CreatedAt = DateTime.Now;
                    item.UpdatedAt = DateTime.Now;
                    await items.AddAsync(item);
                    return RedirectToAction("Items", "Item");
                }
                if (!ModelState.IsValid)
                {
                    if (item.ImageFile == null)
                    {
                        ModelState.AddModelError("ImageFile", "The Image field is required.");
                    }
                    return View(item);
                }
            }
            else
            {
                var existingItem = await items.GetByIdAsync(item.ItemId, new QueryOptions<Item> { Includes = "Category" });
                if (existingItem is null)
                {
                    ModelState.AddModelError("", "No Item Found.");
                    ViewBag.Categories = await categories.GetAllAsync();
                    return View(item);
                }

                string oldImageUrl = existingItem.ImageUrl;
                string newImageUrl = "";
                if (!ModelState.IsValid)
                {
                    ViewData["ImageUrl"] = newImageUrl;
                    return View(item);
                }
                
                if(item.ImageFile != null)
                {
                    string newImageUploader = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    newImageUrl += Guid.NewGuid().ToString() + "_" + item.ImageFile.FileName;
                    string newFilePath = Path.Combine(newImageUploader, newImageUrl);
                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await item.ImageFile.CopyToAsync(fileStream);
                    }

                    string oldFilePath = Path.Combine(newImageUploader, oldImageUrl);
                    System.IO.File.Delete(oldFilePath);
                }                

                existingItem.ItemName = item.ItemName;
                existingItem.Description = item.Description;
                existingItem.Price = item.Price;
                existingItem.StockQuantity = item.StockQuantity;
                existingItem.ImageUrl = newImageUrl;
                existingItem.CategoryId = CatId;
                existingItem.UpdatedAt = DateTime.Now;
                try
                {
                    await items.UpdateAsync(existingItem);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error = {ex.GetBaseException().Message}");
                    ViewBag.Categories = await categories.GetAllAsync();
                    return View(item);
                }
            }
            return RedirectToAction("Items", "Item");
        }
    }
}
