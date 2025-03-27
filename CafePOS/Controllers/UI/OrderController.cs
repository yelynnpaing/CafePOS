using CafePOS.Data;
using CafePOS.Models;
using CafePOS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Reflection.Metadata.Ecma335;

namespace CafePOS.Controllers.UI
{
    [Authorize]
    [Route("mm-cafe/order/")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private Repository<Item> _items;
        private Repository<Order> _orders;
        private Repository<OrderItem> _orderItems;
        private Repository<Category> _categories;
        private Repository<CafeTable> _cafeTables;
        private readonly UserManager<Users> _userManager;

        public OrderController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _items = new Repository<Item>(context);
            _orders = new Repository<Order>(context);
            _orderItems = new Repository<OrderItem>(context);
            _categories = new Repository<Category>(context);
            _cafeTables = new Repository<CafeTable>(context);
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

        [Route("viewCart")]
        [HttpGet]
        public async Task<IActionResult> ViewCart()
        {
            var TableList = await _cafeTables.GetAllAsync();
            ViewBag.CafeTables = new SelectList(TableList, "CafeTableId", "TableNumber");
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if(model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }

            return View(model);
        }
        
        [Route("placeOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string orderType, string orderStatus, Guid cafeTableId, string paymentStatus)
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
                CafeTableId = cafeTableId,
                //TableNumber = tableNumber,
                PaymentStatus = paymentStatus,
                Notes = "Waiting Payment",
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

        [Route("view")]
        [HttpGet]
        public async Task<IActionResult> View()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            var userOrders = await _orders.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Item"
            });
            ViewBag.CafeTables = await _cafeTables.GetAllAsync();

            return View(userOrders);
        }

        [Route("editOrder")]
        [HttpGet]
        public async Task<IActionResult> EditOrder(Guid id)
        {
            var TableList = await _cafeTables.GetAllAsync();
            ViewBag.CafeTables = new SelectList(TableList, "CafeTableId", "TableNumber");
            var order = await _orders.GetByIdAsync(id, new QueryOptions<Order> { Includes = "OrderItems, OrderItems.Item" });            
            return View(order);
        }

        
        [HttpGet]
        public async Task<IActionResult> RemoveOrderItem(Guid orderId, Guid itemId)
        {
            var order = await _orders.GetByIdAsync(orderId, new QueryOptions<Order> { Includes = "OrderItems, OrderItems.Item" });
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == itemId);

            order.OrderItems.Remove(orderItem);            
            await _orderItems.DeleteAsync(orderItem.OrderItemId);
            order.TotalAmount = order.TotalAmount - (orderItem.UnitPrice * orderItem.Quantity);
            await _orders.UpdateAsync(order);
            return RedirectToAction("View");
        }

        [Route("updateOrder")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Guid OrderId, string orderType, string orderStatus, Guid cafeTableId, string paymentStatus)
        {
            var model = await _orders.GetByIdAsync(OrderId, new QueryOptions<Order> { Includes = "OrderItems, OrderItems.Item, CafeTable" });
            if (model is null) return NotFound();
            model.UpdatedAt = DateTime.Now;
            model.OrderType = orderType;
            model.OrderStatus = orderStatus;
            model.CafeTableId = cafeTableId;
            model.PaymentStatus = paymentStatus;
            model.Notes = "Waiting Payment";

            await _orders.UpdateAsync(model);            
            return RedirectToAction("View");
        }

        [Route("makePayment")]
        [HttpGet]
        public async Task<IActionResult> MakePayment(Guid id)
        {
            var order = await _orders.GetByIdAsync(id, new QueryOptions<Order> { Includes = "OrderItems, OrderItems.Item" });
            ViewBag.CafeTables = await _cafeTables.GetAllAsync();           
            return View(order);
        }

        [Route("makePayment")]
        [HttpPost]
        public async Task<IActionResult> MakePayment(Guid OrderId, string orderType, string orderStatus, string paymentStatus, string notes)
        {
            var model = await _orders.GetByIdAsync(OrderId, new QueryOptions<Order> { Includes = "OrderItems, OrderItems.Item, CafeTable" });
            if (model is null) return NotFound();
            model.UpdatedAt = DateTime.Now;
            model.OrderType = orderType;
            model.OrderStatus = orderStatus;
            //model.CafeTableId = cafeTableId;
            model.PaymentStatus = paymentStatus;
            model.Notes = notes;

            await _orders.UpdateAsync(model);
            return RedirectToAction("View");
        }

    }
}

