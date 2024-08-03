using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using PelicanManagementUi.ViewModels.Account;

namespace PelicanManagementUi.Controllers
{
    [AllowAnonymous]

    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotyfService _toastNotification;

        public AccountController(IUserService userService, INotyfService notyfService)
        {
            _userService = userService;
            _toastNotification = notyfService;
        }


        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateViewModel model)
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





        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string phoneNumber)
        {
            var result = await _userService.SendSmsForChangePassword(phoneNumber);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("ForgotPassword", "Account");
            }
            _toastNotification.Success(result.Message);
            TempData["PhoneNumber"] = phoneNumber;
            return RedirectToAction("ConfirmOtp", "Account");

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOtp()
        {
            var phoneNumber = TempData["PhoneNumber"] as string;
            if(string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new ForgetPasswordPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOtp(ConfrimOtpViewModel model)
        {
            var result = await _userService.ConfirmOtp(model);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("ConfirmOtp", "Account");
            }
            _toastNotification.Success(result.Message);
            TempData["PhoneNumber"] = model.PhoneNumber;
            return RedirectToAction("SubmitPassword", "Account");
        }



        [HttpGet]
        public async Task<IActionResult> SubmitPassword()
        {
            var phoneNumber = TempData["PhoneNumber"] as string;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new ForgetPasswordPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }


        [HttpPost]
        public async Task<IActionResult> SubmitPassword(ForgetPasswordViewModel model)
        {
            var result = await _userService.SubmitPasswod(model);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("SubmitPassword", "Account");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("Login", "Account");
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
            var userProfile = await _userService.GetUser(Guid.Parse(userId), token);
            return View(userProfile.Data);
        }



        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
