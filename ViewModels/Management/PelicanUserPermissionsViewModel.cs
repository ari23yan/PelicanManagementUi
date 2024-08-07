namespace PelicanManagementUi.ViewModels.Management
{
    public class PelicanUserPermissionsViewModel
    {
        public long Id { get; set; }
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public bool HasPermission { get; set; }
    }
}
