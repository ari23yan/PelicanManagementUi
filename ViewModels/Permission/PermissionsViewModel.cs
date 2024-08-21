namespace UsersManagementUi.ViewModels.Permission
{
    public class PermissionsViewModel
    {
        public Guid Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionName_Farsi { get; set; }
        public string? Description { get; set; }
        public bool HasPermission { get; set; }
    }
}
