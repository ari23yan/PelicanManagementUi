using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.Models;
using System.Diagnostics;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
