using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.ViewModels.User;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly INotyfService _toastNotification;

        public RoleController(IRoleService roleService, INotyfService notyfService)
        {
            _roleService = roleService;
            _toastNotification = notyfService;

        }
        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var roles = await _roleService.GetRoleList(model, token);
            if (!roles.IsSuccessFull.Value)
            {
                _toastNotification.Error(roles.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<RolesListViewModel>
            {
                Data = roles.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = roles.TotalCount.Value
            };
            return View(paginationModel);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _roleService.GetRole(id, token);
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Role");
            }
            return View(userDetail.Data);
        }
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _roleService.GetRolePermissionsAndMenus(null, token);
            return View(userDetail.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRoleViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _roleService.AddRole(model, token);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Add", "Role");

            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Role");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _roleService.UpdateRole(model, token);
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Detail", "Role", new { id = model.RoleId });
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("Detail", "Role", new { id = model.RoleId });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _roleService.DeleteRole(id, token);
            return Json(result);
        }
        [HttpPut]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _roleService.ToggleRoleActiveStatus(id, token);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetRolePermissions(Guid roleID)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _roleService.GetRolePermissions(roleID, token);
            return Json(userDetail.Data);
        }

        [HttpPost]
        public async Task<IActionResult> GetRolePermissionsAndMenus(Guid roleID)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _roleService.GetRolePermissionsAndMenus(roleID, token);
            return Json(userDetail.Data);
        }

    }
}
