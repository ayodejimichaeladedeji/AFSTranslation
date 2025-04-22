using Microsoft.AspNetCore.Mvc;

namespace AFSTranslator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}