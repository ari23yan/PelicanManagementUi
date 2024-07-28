using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.WebServices.Interfaces;
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
            return View(users.Data);
        }
    }
}
