namespace UsersManagementUi.ViewModels.Management
{
    public class HisNovinUserListViewModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; } = false;
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; } = false;
        public bool? TwoFactorEnabled { get; set; } = false;
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool? LockoutEnabled { get; set; } = true;
        public int? AccessFailedCount { get; set; } = 0;
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? PersonalCode { get; set; }
    }
}
