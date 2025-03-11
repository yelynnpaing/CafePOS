using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{
    public class AdminPanelController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }


    }
}
