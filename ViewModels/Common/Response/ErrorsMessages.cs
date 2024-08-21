using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagementUi.ViewModels.Common.Response
{
    public class ErrorsMessages
    {
        public static string FaildLogin = "خطا، لطفاً دوباره تلاش کنید";
        public static string BadApiResponse = "خطا، پاسخ API نامعتبر است";
        public static string SuccessLogin = "ورود موفقیت‌آمیز";
        public static string UserORPasswrodIsWrong = "نام کاربری یا رمز عبور اشتباه است";
        public static string NotAuthenticated = "کاربر احراز هویت نشده است";
        public static string Authenticated = "کاربر احراز هویت شده است";
        public static string NotFound = "موردی یافت نشد";
        public static string SuccessRegister = "ثبت‌نام موفقیت‌آمیز بود";
        public static string UserNotfound = "کاربر یافت نشد، لطفاً دوباره تلاش کنید";
        public static string NotActive = "حساب کاربری غیرفعال است";
        public static string PassswordAndConfrimPassswordIsnotEqueal = "رمز عبور و تکرار آن مطابقت ندارند، لطفاً دوباره تلاش کنید";
        public static string UserExist = "کاربری با این مشخصات قبلاً وجود دارد";
        public static string Exist = "رکوردی با این مشخصات قبلاً وجود دارد";
        public static string NullInputs = "مقادیر ورودی خالی هستند";
        public static string Success = "عملیات موفقیت‌آمیز بود";
        public static string Faild = "عملیات ناموفق بود";
        public static string OtpIncorrect = "کد تایید اشتباه است";
        public static string EmailSend = "کد تایید به ایمیل شما ارسال شد";
        public static string PermissionDenied = "شما دسترسی مورد نیاز برای انجام این عملیات را ندارید";
        public static string InternalServerError = "اوه، خطای سرور داخلی";
        public static string PhoneNumberAlreadyExist = "شماره تلفن در پایگاه داده موجود است";
        public static string UsernameAlreadyExist = "نام کاربری در پایگاه داده موجود است";
        public static string SmsPanelNotResponding = "پیامک ارسال نشد، خطای سرور داخلی فراپیامک";
    }
}
