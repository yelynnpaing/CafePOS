using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.AdminPanel
{
    [Authorize]
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
