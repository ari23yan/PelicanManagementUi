using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UsersManagementUi.Enums
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
        [Display(Name = "ویرایش نقش")]
        UpdateRole,
        [Display(Name = "حذف نقش")]
        DeleteRole,
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


        [Display(Name = " ایجاد کاربر کلینیک")]
        CreateClinicUser,
        [Display(Name = "حذف کاربر کلینیک")]
        DeleteClinicUser,
        [Display(Name = "ویرایش کاربر کلینیک")]
        UpdateClinicUser,

        [Display(Name = " ایجاد کاربر اچ آی اس نوین")]
        CreateHisNovinUser,
        [Display(Name = "حذف کاربر اچ آی اس نوین")]
        DeleteHisNovinUser,
        [Display(Name = "ویرایش کاربر اچ آی اس نوین")]
        UpdateHisNovinUser,
    }
}
