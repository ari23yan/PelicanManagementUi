using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Data;
using System.Drawing.Printing;
using System.Reflection;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly INotyfService _toastNotification;

        public UserController(IUserService userService, IRoleService roleService, INotyfService notyfService)
        {
            _userService = userService;
            _roleService = roleService;
            _toastNotification = notyfService;

        }
        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _userService.GetUserList(model, token);
            if (users.Status == "Unauthorized")
            {
                _toastNotification.Information(users.Message);

                return RedirectToAction("SignOut", "Account");
            }
            if (!users.IsSuccessFull.Value)
            {
                _toastNotification.Error(users.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<UsersListViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            return View(paginationModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _userService.GetUser(id, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "User");
            }
            return View(userDetail.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var rolesList = await _roleService.GetRolesList(token);
            return View(rolesList.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _userService.AddUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                var rolesList = await _roleService.GetRolesList(token);
                return View(rolesList.Data);
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "User");

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _userService.UpdateUser(model, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("Detail", "User", new { id = model.UserId });
            }
            _toastNotification.Success(userDetail.Message);
            return RedirectToAction("Detail", "User", new { id = model.UserId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _userService.DeleteUser(id, token);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _userService.ToggleActiveStatus(id, token);
            return Json(result);
        }




    }
}
