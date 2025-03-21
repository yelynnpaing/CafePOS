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
        private readonly UserManager<Users> _userManager;

        public OrderController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _items = new Repository<Item>(context);
            _orders = new Repository<Order>(context);
            _categories = new Repository<Category>(context);
            _userManager = userManager;
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

        [Authorize]
        [Route("placeOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string orderType, string orderStatus, int tableNumber)
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if(model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }

            //create a new order entity
            Order order = new Order
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                OrderType = orderType,
                OrderStatus = orderStatus,
                TableNumber = tableNumber,
                TotalAmount = model.TotalAmount,
                UserId = Guid.Parse(_userManager.GetUserId(User))
            };
            //add order items into order entity
            foreach(var item in model.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ItemId = item.ItemId,                    
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }
            //save order entity to the database
            await _orders.AddAsync(order);
            HttpContext.Session.Remove("OrderViewModel");
            return RedirectToAction("View");
        }

        [Authorize]
        [Route("view")]
        [HttpGet]
        public async Task<IActionResult> View()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            var userOrders = await _orders.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Item"
            });

            return View(userOrders);
        }

    }
}

