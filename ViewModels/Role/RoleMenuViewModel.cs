using UsersManagementUi.ViewModels.Permission;
using System.Security;

namespace UsersManagementUi.ViewModels.Role
{
    public class RoleMenuViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public string Description { get; set; }
        public List<RoleMenusViewModel> Menus { get; set; }
        public List<PermissionsViewModel> Permission { get; set; }

    }
}
