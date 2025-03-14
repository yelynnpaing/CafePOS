using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers.UI
{
    [Route("mm-cafe")]
    public class UIPanelController : Controller
    {
        [Route("home")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
