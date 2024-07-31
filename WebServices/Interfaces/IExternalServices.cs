using PelicanManagementUi.ViewModels;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Permission;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.ViewModels.User;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IExternalServices
    {
        Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel);

        #region User Services

        Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token);
        Task<ResponseViewModel<UserDetailViewModel>> GetUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> UpdateUser(UpdateUserViewModel updateUserViewModel, string token);
        Task<ResponseViewModel<bool>> AddUser(AddUserViewModel addUserViewModel, string token);
        Task<ResponseViewModel<bool>> DeleteUser(Guid userId, string token);
        Task<ResponseViewModel<bool>> ToggleActiveStatus(Guid userId, string token);

        #endregion

        #region Role Services
        Task<ResponseViewModel<List<RolesListViewModel>>> GetRoleList(PaginationViewModel model, string token);
        Task<ResponseViewModel<GetRoleMenuViewModel>> GetRoleMenu(Guid roleId, string token);
        Task<ResponseViewModel<List<RolesListViewModel>>> GetRolesList(string token);
        Task<ResponseViewModel<List<PermissionsViewModel>>> GetRolePermissions(Guid roleId, string token);
        Task<ResponseViewModel<bool>> DeleteRole(Guid roleId, string token);
        Task<ResponseViewModel<bool>> ToggleRoleActiveStatus(Guid roleId, string token);
        Task<ResponseViewModel<bool>> AddRole(AddRoleViewModel addRoleViewModel, string token);
        Task<ResponseViewModel<bool>> UpdateRole(UpdateRoleViewModel updateUserViewModel, string token);
        #endregion

    }
}
