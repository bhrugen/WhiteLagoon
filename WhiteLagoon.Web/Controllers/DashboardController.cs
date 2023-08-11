using Microsoft.AspNetCore.Mvc;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
