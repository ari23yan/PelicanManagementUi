using UsersManagementUi.ViewModels;
using UsersManagementUi.ViewModels.Account;
using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Common.Response;
using UsersManagementUi.ViewModels.User;
using UsersManagementUi.ViewModels.UserActivity;

namespace UsersManagementUi.WebServices.Interfaces
{
    public interface IUserService
    {
        Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel);
        Task<ResponseViewModel<bool>> SendSmsForChangePassword(string mobile);
        Task<ResponseViewModel<bool>> ConfirmOtp(ConfrimOtpViewModel viewModel);
        Task<ResponseViewModel<bool>> SubmitPasswod(ForgetPasswordViewModel viewModel);
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token);
        Task<ResponseViewModel<List<UserActivityViewModel>>> GetUserActivitesList(PaginationViewModel model, string token);
        Task<ResponseViewModel<UserDetailViewModel>> GetUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> UpdateUser(UpdateUserViewModel updateUserViewModel, string token);
        Task<ResponseViewModel<bool>> AddUser(AddUserViewModel addUserViewModel, string token);
        Task<ResponseViewModel<bool>> DeleteUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> ToggleActiveStatus(Guid userId, string token);
        Task<ResponseViewModel<bool>> ChangePassword(string password, string token);
    }
}
