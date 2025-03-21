using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{

    [Route("mm-cafe/items/")]
    public class UIItemController : Controller
    {
        private Repository<Item> _items;
        private Repository<Category> _categories;

        public UIItemController(AppDbContext context)
        {
            _items = new Repository<Item>(context);
            _categories = new Repository<Category>(context);
        }

        [Authorize]
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List(string searchString, Guid? CategoryId)
        {
            var Items = await _items.GetAllAsync();
            var Categories = await _categories.GetAllAsync();

            //use category filter if a category is selected
            if (CategoryId.HasValue)
            {
                Items = Items.Where(i => i.CategoryId == CategoryId.Value).ToList();
            }

            // use search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                Items = Items.Where(i => i.ItemName.Contains(searchString)).ToList();
            }

            // use Group filtered items by category
            var groupedItems = Categories.Select(category => new
            {
                Category = category,
                Items = Items.Where(i => i.CategoryId == category.CategoryId).ToList()
            }).ToList();

            ViewBag.GroupItems = groupedItems;
            ViewBag.SearchString = searchString;
            ViewBag.Categories = Categories;
            ViewBag.SelectedCategoryId = CategoryId;

            return View();
        }
                
    }
}
