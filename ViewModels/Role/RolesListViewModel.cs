namespace PelicanManagementUi.ViewModels.Role
{
    public class RolesListViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string RoleName_Farsi { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
