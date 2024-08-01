using Microsoft.AspNetCore.Mvc;
using PelicanManagementUi.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Security.Claims;

namespace PelicanManagementUi.ViewComponents
{
    public class SideBarViewComponent: ViewComponent
    {

        private readonly IRoleService _roleService;
        public SideBarViewComponent(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roleId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Role));
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var fullName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var menu = await _roleService.GetRoleMenu(roleId,token);
            var ViewModel = new UserViewModel { FullName = fullName, RoleData = menu.Data };
            return View("SideBar", ViewModel);
        }
    }
}
