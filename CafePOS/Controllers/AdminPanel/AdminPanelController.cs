using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{
    [Route("admin")]
    public class AdminPanelController : Controller
    {
        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


    }
}
