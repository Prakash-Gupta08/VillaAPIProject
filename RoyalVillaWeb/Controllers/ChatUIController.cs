using Microsoft.AspNetCore.Mvc;

namespace RoyalVillaWeb.Controllers
{
    public class ChatUIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
