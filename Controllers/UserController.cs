using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.Models.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Drawing.Printing;
using System.Reflection;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IExternalServices _service;
        public UserController(IExternalServices externalServices)
        {
            _service = externalServices;
        }
        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _service.GetUserList(model, token);

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
            var userDetail = await _service.GetUser(id, token);
            return View(userDetail.Data);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _service.DeleteUser(id, token);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _service.ToggleActiveStatus(id, token);
            return Json(result);
        }
    }
}
