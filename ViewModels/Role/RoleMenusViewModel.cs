namespace PelicanManagementUi.ViewModels.Role
{
    public class RoleMenusViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Name_Farsi { get; set; }
        public string? Description { get; set; }
        public Guid? SubMenuId { get; set; }
        public bool HasMenu { get; set; } = false;
        public string Link { get; set; }
        public List<RoleMenusViewModel> SubMenus { get; set; }
    }
}
