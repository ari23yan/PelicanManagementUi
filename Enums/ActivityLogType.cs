using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PelicanManagementUi.Enums
{
    public enum ActivityLogType : long
    {
        [Display(Name = "ایجاد کاربر")]
        CreateUser = 1,
        [Display(Name = "حذف کاربر")]
        DeleteUser,
        [Display(Name = "ویرایش کاربر")]
        UpdateUser,
        [Display(Name = "فعال سازی/غیرفعال سازی کاربر")]
        ActiveOrDeActiveUser,
        [Display(Name = "ایجاد نقش")]
        CreateRole,
        [Display(Name = "حذف نقش")]
        DeleteRole,
        [Display(Name = "ویرایش نقش")]
        UpdateRole,
        [Display(Name = "فعال سازی/غیرفعال سازی نقش")]
        ActiveOrDeActiveRole = 9,
        [Display(Name = " ایجاد کاربر پلیکان")]
        CreatePelicanUser,
        [Display(Name = "حذف کاربر پلیکان")]
        DeletePelicanUser,
        [Display(Name = "ویرایش کاربر پلیکان")]
        UpdatePelicanUser,

        [Display(Name = " ایجاد کاربر تریاژ")]
        CreateTeriageUser,
        [Display(Name = "حذف کاربر تریاژ")]
        DeleteTeriageUser,
        [Display(Name = "ویرایش کاربر تریاژ")]
        UpdateTeriageUser,
    }
}
