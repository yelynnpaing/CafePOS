using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
