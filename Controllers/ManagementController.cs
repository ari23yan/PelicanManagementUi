using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.ViewModels.Common.Pagination;
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
            var roles = await _managementService.GetUsersList(model, token);
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
    }
}
