using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Management;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.ViewModels.User;
using UsersListViewModel = PelicanManagementUi.ViewModels.Management.UsersListViewModel;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IManagementService
    {
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUsersList(PaginationViewModel model, string token);
        Task<ResponseViewModel<bool>> DeleteUser(int userId, string token);
        Task<ResponseViewModel<IdentityUserDetailViewModel>> GetUser(string username, string token);
        Task<ResponseViewModel<bool>> AddUser(AddIdentityUserViewModel addUserViewModel, string token);
        Task<ResponseViewModel<bool>> UpdateUser(UpdatePelicanUserViewModel updatePelicanUserViewModel, string token);
        Task<ResponseViewModel<PermissionAndUnitsViewModel>> GetPermissionsAndUnits(string token);

    }
}
