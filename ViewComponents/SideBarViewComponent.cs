using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.Models.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;

namespace PelicanManagementUi.ViewComponents
{
    public class SideBarViewComponent: ViewComponent
    {

        private readonly IExternalServices _service;
        public SideBarViewComponent(IExternalServices services)
        {
                _service = services;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roleId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Role));
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var fullName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var menu = await _service.GetRoleMenu(roleId,token);

            var ViewModel = new UserViewModel { FullName = fullName, RoleData = menu.Data };
            return View("SideBar", ViewModel);
        }
    }
}
