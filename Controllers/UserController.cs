using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.Models.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Drawing.Printing;
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
        public async Task<IActionResult> Index(PaginationViewModel model)
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
    }
}
