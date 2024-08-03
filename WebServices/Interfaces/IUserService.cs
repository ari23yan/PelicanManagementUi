using PelicanManagementUi.ViewModels;
using PelicanManagementUi.ViewModels.Account;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.User;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IUserService
    {
        Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel);
        Task<ResponseViewModel<bool>> SendSmsForChangePassword(string mobile);
        Task<ResponseViewModel<bool>> ConfirmOtp(ConfrimOtpViewModel viewModel);
        Task<ResponseViewModel<bool>> SubmitPasswod(ForgetPasswordViewModel viewModel);
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token);
        Task<ResponseViewModel<UserDetailViewModel>> GetUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> UpdateUser(UpdateUserViewModel updateUserViewModel, string token);
        Task<ResponseViewModel<bool>> AddUser(AddUserViewModel addUserViewModel, string token);
        Task<ResponseViewModel<bool>> DeleteUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> ToggleActiveStatus(Guid userId, string token);
        Task<ResponseViewModel<bool>> ChangePassword(string password, string token);
    }
}
