using UsersManagementUi.Enums;
using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Common.Response;
using UsersManagementUi.ViewModels.Management;
using UsersManagementUi.ViewModels.Role;
using UsersManagementUi.ViewModels.User;
using UsersListViewModel = UsersManagementUi.ViewModels.Management.UsersListViewModel;

namespace UsersManagementUi.WebServices.Interfaces
{
    public interface IManagementService
    {
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUsersList(PaginationViewModel model, string token,UserType type);
        Task<ResponseViewModel<bool>> DeleteUser(int userId, string token);
        Task<ResponseViewModel<IdentityUserDetailViewModel>> GetUser(string username, string token);
        Task<ResponseViewModel<UsersListViewModel>> GetTeriageUser(string username, string token);
        Task<ResponseViewModel<bool>> AddUser(AddIdentityUserViewModel addUserViewModel, string token, UserType type);
        Task<ResponseViewModel<bool>> UpdateUser(UpdatePelicanUserViewModel updatePelicanUserViewModel, string token);
        Task<ResponseViewModel<PermissionAndUnitsViewModel>> GetPermissionsAndUnits(string token);

    }
}
