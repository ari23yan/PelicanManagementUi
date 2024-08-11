using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.UserActivity;
using PelicanManagementUi.WebServices.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IUserService _userService;
        private readonly INotyfService _toastNotification;

        public HomeController(IUserService userService, INotyfService notyfService)
        {
            _userService = userService;
            _toastNotification = notyfService;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logs(PaginationViewModel model) 
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _userService.GetUserActivitesList(model, token);
            if (!users.IsSuccessFull.Value)
            {
                _toastNotification.Error(users.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<UserActivityViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            return View(paginationModel);
        }

        [AllowAnonymous]
        [Route("NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }


    }
}
