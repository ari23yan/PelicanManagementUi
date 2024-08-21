namespace UsersManagementUi.ViewModels.UserActivity
{
    public class UserActivityViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public long UserActivityLogTypeId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Description { get; set; }
    }
}
