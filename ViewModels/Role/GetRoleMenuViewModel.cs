namespace PelicanManagementUi.ViewModels.Role
{
    public class GetRoleMenuViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public List<RoleMenusViewModel> Menus { get; set; }
    }
}
