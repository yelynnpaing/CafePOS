using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    [Authorize]
    [Route("mm-cafe/")]
    public class UIPanelController : Controller
    {        
        [Route("home")]
        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
    }
}
