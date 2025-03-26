using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    [Authorize]
    [Route("mm-cafe/")]
    public class UIPanelController : Controller
    {
        private readonly AppDbContext _context;
        private Repository<Item> _items;
        private Repository<CafeTable> _cafeTables;

        public UIPanelController(AppDbContext context)
        {
            _context = context;
            _items = new Repository<Item>(context);
            _cafeTables = new Repository<CafeTable>(context);
        }

        [Route("home")]
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var CafeTables = await _cafeTables.GetAllAsync();            
            return View(CafeTables);
        }
    }
}
