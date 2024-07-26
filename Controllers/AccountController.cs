using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PelicanManagementUi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
