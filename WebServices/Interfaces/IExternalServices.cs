using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.Models.ViewModels.Common.Response;
using PelicanManagementUi.Models.ViewModels.Permission;
using PelicanManagementUi.Models.ViewModels.Role;
using PelicanManagementUi.Models.ViewModels.User;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IExternalServices
    {

        // User Managment
        Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel);
        Task<ResponseViewModel<GetRoleMenuViewModel>> GetRoleMenu(Guid roleId,string token);
        Task<ResponseViewModel<List<RolesListViewModel>>> GetRolesList(string token);
        Task<ResponseViewModel<List<PermissionsViewModel>>> GetRolePermissions(Guid roleId,string token);
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token);
        Task<ResponseViewModel<UserDetailViewModel>> GetUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> UpdateUser(UpdateUserViewModel updateUserViewModel, string token);
        Task<ResponseViewModel<bool>> AddUser(AddUserViewModel updateUserViewModel, string token);
        Task<ResponseViewModel<bool>> DeleteUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> ToggleActiveStatus(Guid userId, string token);
        // Pelican
    }
}
