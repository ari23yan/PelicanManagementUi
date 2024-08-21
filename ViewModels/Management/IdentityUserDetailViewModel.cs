namespace UsersManagementUi.ViewModels.Management
{
    public class IdentityUserDetailViewModel
    {
        public AddIdentityUserViewModel User { get; set; }
        public List<PelicanUserUnitsViewModel> UserUnits { get; set; }
        public List<PelicanUserPermissionsViewModel> UserPermissions { get; set; }
    }
}
