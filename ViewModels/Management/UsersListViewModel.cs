using Microsoft.AspNetCore.Identity;

namespace UsersManagementUi.ViewModels.Management
{
    public class UsersListViewModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? PersonalCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
