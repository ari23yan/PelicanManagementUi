using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> List(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _managementService.GetUsersList(model, token);
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
        public async Task<IActionResult> Detail(string username)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetUser(username, token);
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Management");
            }
            return View(userDetail.Data);
        }


        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetPermissionsAndUnits(token);
            return View(userDetail.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddIdentityUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.AddUser(model, token);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Add", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.DeleteUser(id, token);
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdatePelicanUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.UpdateUser(model, token);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("List", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management");
        }





    }
}
