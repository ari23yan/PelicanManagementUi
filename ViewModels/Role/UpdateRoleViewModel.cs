namespace PelicanManagementUi.ViewModels.Role
{
    public class UpdateRoleViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public string? Description { get; set; }
        public Guid[] PermissionIds { get; set; }
        public Guid[] MenuIds { get; set; }
    }
}
