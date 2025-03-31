using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    [Authorize]
    //[Route("mm-cafe")]
    public class UIPanelController : Controller
    {
        private readonly AppDbContext _context;
        private Repository<Item> _items;
        private Repository<CafeTable> _cafeTables;
        private Repository<Order> _orders;
        private Repository<OrderItem> _orderItems;

        public UIPanelController(AppDbContext context)
        {
            _context = context;
            _items = new Repository<Item>(context);
            _cafeTables = new Repository<CafeTable>(context);
            _orders = new Repository<Order>(context);
            _orderItems = new Repository<OrderItem>(context);
        }

        //[Route("home")]
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var CafeTables = await _cafeTables.GetAllAsync();
            ViewBag.Items = await _items.GetAllAsync();
            ViewBag.Orders = await _orders.GetAllAsync();
            return View(CafeTables);
        }
    }
}
