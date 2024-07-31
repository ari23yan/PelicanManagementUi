namespace PelicanManagementUi.ViewModels.User
{
    public class AddUserViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid RoleId { get; set; }
    }
}
