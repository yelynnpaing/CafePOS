using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CafePOS.Controllers.UI
{
    [Route("mm-cafe/order/")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private Repository<Item> _items;
        private Repository<Order> _orders;
        private Repository<Category> _categories;
        private readonly UserManager<Users> _users;

        public OrderController(AppDbContext context, UserManager<Users> users)
        {
            _context = context;
            _items = new Repository<Item>(context);
            _orders = new Repository<Order>(context);
            _categories = new Repository<Category>(context);
            _users = users;
        }

        [Authorize]
        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Items = await _items.GetAllAsync(),                
            };
            return View(model);
        }

        [Authorize]
        [Route("addItem")]
        [HttpPost]
        public async Task<IActionResult> AddItem(Guid itemId, int itemQty)
        {
            var item = await _context.Items.FindAsync(itemId);
            if(item is null)
            {
                return NotFound();
            }

            //retrieve or create an OrderViewModel from session or other state management
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Items = await _items.GetAllAsync()
            };

            //check if the item already in the order
            var existingItem = model.OrderItems.FirstOrDefault(oi => oi.ItemId == itemId);

            //if the item is already in the order and update quantity
            if(existingItem != null)
            {
                existingItem.Quantity += itemQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Quantity = itemQty,
                    Price = item.Price,
                });
            }

            //Update total price
            model.TotalAmount = model.OrderItems.Sum(oi => oi.Quantity * oi.Price);

            //Save OrderViewModel to the session
            HttpContext.Session.Set("OrderViewModel", model);
            return RedirectToAction("Create", model);
        }

        [Authorize]
        [Route("viewCart")]
        [HttpGet]
        public IActionResult ViewCart()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if(model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }
    }
}

