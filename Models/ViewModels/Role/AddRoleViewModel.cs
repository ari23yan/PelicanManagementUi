namespace PelicanManagementUi.Models.ViewModels.Role
{
    public class AddRoleViewModel
    {
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public string? Description { get; set; }
        public Guid[] PermissionIds { get; set; }
        public Guid[] MenuIds { get; set; }
    }
}
