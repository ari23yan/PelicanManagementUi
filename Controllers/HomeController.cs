using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi;
using System.Diagnostics;

namespace PelicanManagementUi.Controllers
{
    public class HomeController : Controller
    {
    [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
