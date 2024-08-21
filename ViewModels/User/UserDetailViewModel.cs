using UsersManagementUi.ViewModels.Permission;
using UsersManagementUi.ViewModels.Role;
using System.Security;

namespace UsersManagementUi.ViewModels.User
{
    public class UserDetailViewModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid? IsCreatedBy { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsModified { get; set; } = false;
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public List<PermissionsViewModel> Permissions { get; set; }
        public List<RoleMenusViewModel> Menus { get; set; }
        public List<RolesListViewModel> AllRoles { get; set; }
        public List<UserActivityLogViewModel> UserActivities { get; set; }

    }



}
