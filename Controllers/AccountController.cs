using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using PelicanManagementUi.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PelicanManagementUi.Controllers
{
    [Authorize]

    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotyfService _toastNotification;

        public AccountController(IUserService userService, INotyfService notyfService)
        {
            _userService = userService;
            _toastNotification = notyfService;
        }

        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async  Task<IActionResult> Login(AuthenticateViewModel model)
        {
            var authentication = await _userService.Authenticate(model);
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
                _toastNotification.Success(ErrorsMessages.Success);
                return RedirectToAction("Index", "Home");
            }
            _toastNotification.Error(authentication.Message);
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePasswod(string password)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _userService.ChangePassword(password, token);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Profile", "Account");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("Profile", "Account");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _userService.GetUser(Guid.Parse(userId),token);
            return View(userProfile.Data);
        }
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
