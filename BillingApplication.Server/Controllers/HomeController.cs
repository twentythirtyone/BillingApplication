using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
