using UsersManagementUi.Enums;
using System.ComponentModel.DataAnnotations;

namespace UsersManagementUi.ViewModels.User
{
    public class UserActivityLogViewModel
    {
        public Guid UserId { get; set; }
        [MaxLength(100)]
        public ActivityLogType UserActivityLogTypeId { get; set; }
        public DateTime Timestamp { get; set; }
        [MaxLength(5000)]
        public string OldValues { get; set; }
        [MaxLength(5000)]
        public string NewValues { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
    }
}
