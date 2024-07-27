using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Response;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace PelicanManagementUi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IExternalServices _service;
        private readonly INotyfService _toastNotification;

        public AccountController(IExternalServices externalServices, INotyfService notyfService)
        {
            _service = externalServices;
            _toastNotification = notyfService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Login(AuthenticateViewModel model)
        {
            var authentication = await _service.Authenticate(model);
            if (authentication.IsSuccessFull.HasValue && authentication.IsSuccessFull.Value)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,authentication.Data.FullName.ToString()),
                    new Claim(ClaimTypes.Role, authentication.Data.RoleId.ToString()),
                    new Claim(ClaimTypes.Email, authentication.Data.Email.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, authentication.Data.Id.ToString()),
                    new Claim(ClaimTypes.Authentication, authentication.Data.Token.ToString())
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties { IsPersistent = true };
                HttpContext.SignInAsync(principal, properties);
                //_toastNotification.Success(ErrorsMessages.Success, 10);
                return RedirectToAction("Index", "Home");
            }
            _toastNotification.Error(authentication.Message, 10);
            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }


        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
