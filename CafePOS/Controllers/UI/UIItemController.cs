using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    [Route("mm-cafe/items/")]
    public class UIItemController : Controller
    {
        private Repository<Item> items;
        private Repository<Category> categories;

        public UIItemController(AppDbContext context)
        {
            items = new Repository<Item>(context);
            categories = new Repository<Category>(context);
        }


        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List(string searchString)
        {
            var Items = await items.GetAllAsync();
            if(!String.IsNullOrEmpty(searchString))
            {
                Items = Items.Where(i => i.ItemName.Contains(searchString)).ToList();
            }
            return View(Items);
        }
    }
}
