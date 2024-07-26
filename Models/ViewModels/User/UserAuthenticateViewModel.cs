using PelicanManagementUi.Models.ViewModels.Role;

namespace PelicanManagementUi.Models.ViewModels.User
{
    public class UserAuthenticateViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public Guid RoleId { get; set; }
        public string Token { get; set; }
    }
}
