using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers
{
    public class ControllerBase : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
