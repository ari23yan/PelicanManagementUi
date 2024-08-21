namespace UsersManagementUi.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
    }
}
