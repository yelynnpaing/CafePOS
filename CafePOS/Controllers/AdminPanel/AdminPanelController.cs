using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CafePOS.Controllers.AdminPanel
{
    [Authorize(Roles ="Admin")]
    [Route("admin/")]
    public class AdminPanelController : Controller
    {
        private Repository<Order> _orders;
        private Repository<Item> _items;
        private Repository<CafeTable> _cafeTables;
        private readonly UserManager<Users> _userManager;
        //private readonly SignInManager<Users> signInManager;

        public AdminPanelController(AppDbContext context, UserManager<Users> userManager)
        {
            _orders = new Repository<Order>(context);
            _items = new Repository<Item>(context);
            _cafeTables = new Repository<CafeTable>(context);           
            _userManager = userManager;
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.AllUsers = _userManager.Users.Count();
            ViewBag.Items = await _items.GetAllAsync();
            ViewBag.CafeTables = await _cafeTables.GetAllAsync();
            ViewBag.Orders = await _orders.GetAllAsync();
            return View();
        }

        [Route("orders")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var Orders = await _orders.GetAllAsync();
            var sortOrders = Orders.OrderByDescending(o => o.CreatedAt).ToList();
            ViewBag.CafeTables = await _cafeTables.GetAllAsync();
            var Users = await _userManager.Users.ToListAsync();            

            return View(sortOrders);
        }
    }
}
