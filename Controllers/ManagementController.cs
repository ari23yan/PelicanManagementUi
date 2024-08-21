using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.Enums;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Management;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.WebServices.Implementation;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly IManagementService _managementService;
        private readonly INotyfService _toastNotification;
        public ManagementController(IManagementService managementService, INotyfService notyfService)
        {
            _managementService = managementService;
            _toastNotification = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model, UserType type)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _managementService.GetUsersList(model, token,type);
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
            ViewData["Type"] = (int)type;
            return View(paginationModel);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(string username)
        {
            var paginationModel = new PaginationViewModel();
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetUser(username, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type=2 });
            }
            return View(userDetail.Data);
        }


        [HttpGet]
        public async Task<IActionResult> TeriageUserDetail(string username)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetTeriageUser(username, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type = 1 });
            }
            return View(userDetail.Data);
        }


        public async Task<IActionResult> AddTeriageUser()
        {
            return View();
        }

        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetPermissionsAndUnits(token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            return View(userDetail.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddIdentityUserViewModel model, UserType type)
        {
            var paginationModel = new PaginationViewModel();
            
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.AddUser(model, token,type);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Add", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management", new { model = paginationModel, type });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.DeleteUser(id, token);
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdatePelicanUserViewModel model,int type)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.UpdateUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);

                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type });
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management", new { model = paginationModel, type });
        }





    }
}
