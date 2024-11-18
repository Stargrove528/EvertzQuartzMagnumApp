using Microsoft.AspNetCore.Mvc;

namespace EvertzQuartzMagnumApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
