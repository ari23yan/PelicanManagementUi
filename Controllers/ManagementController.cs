using Microsoft.AspNetCore.Mvc;

namespace PelicanManagementUi.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
