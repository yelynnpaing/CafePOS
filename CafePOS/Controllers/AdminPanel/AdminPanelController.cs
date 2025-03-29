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
    [Authorize]
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
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("orders")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var Orders = await _orders.GetAllAsync();
            var sortOrders = Orders.OrderByDescending(o => o.CreatedAt).ToList();
            ViewBag.CafeTables = await _cafeTables.GetAllAsync();
            //var Users = await _userManager.Users.ToListAsync();
            //var orderWithUser = Orders.Select(order => new
            //{
            //    OrderId = order.OrderId,
            //    UserId = order.UserId,
            //    FullName = Users.FirstOrDefault(u => u.Id == (order.UserId).ToString())?.FullName ?? "Unknown",
            //    OrderType = order.OrderType,
            //    CafeTableId = order.CafeTableId,
            //    TotalAmount = order.TotalAmount,
            //    OrderStatus = order.OrderStatus,
            //    PaymentStatus = order.PaymentStatus,
            //    Notes = order.Notes,
            //    CreatedAt = order.CreatedAt,
            //    UpdatedAt = order.UpdatedAt
            //});
            //ViewData.Model = orderWithUser;
            //return View("Orders", orderWithUser);

            //filter with search string 
            
            return View(sortOrders);
        }

    }
}
