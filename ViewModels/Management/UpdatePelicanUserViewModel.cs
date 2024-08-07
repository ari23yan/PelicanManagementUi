namespace PelicanManagementUi.ViewModels.Management
{
    public class UpdatePelicanUserViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string Password { get; set; }
        public long[] PermissionIds { get; set; }
        public int[] UnitIds { get; set; }
    }
}
