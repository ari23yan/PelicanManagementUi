using PelicanManagementUi.ViewModels.Permission;
using System.Security;

namespace PelicanManagementUi.ViewModels.Role
{
    public class RolesListWithPermissionAndMenusViewModel
    {
        public List<RoleViewModel> Role { get; set; }
        public List<RoleMenusViewModel> Menus { get; set; }
        public List<PermissionsViewModel> Permission { get; set; }
    }
}
