﻿using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Common.Response;
using UsersManagementUi.ViewModels.Permission;
using UsersManagementUi.ViewModels.Role;

namespace UsersManagementUi.WebServices.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseViewModel<List<RolesListViewModel>>> GetRoleList(PaginationViewModel model, string token);
        Task<ResponseViewModel<RoleMenuViewModel>> GetRole(Guid roleId, string token);
        Task<ResponseViewModel<RoleMenuViewModel>> GetRoleMenu(Guid roleId, string token);
        Task<ResponseViewModel<List<RolesListViewModel>>> GetRolesList(string token);
        Task<ResponseViewModel<List<PermissionsViewModel>>> GetRolePermissions(Guid roleId, string token);
        Task<ResponseViewModel<RolesListWithPermissionAndMenusViewModel>> GetRolePermissionsAndMenus(Guid? roleId, string token);
        Task<ResponseViewModel<bool>> DeleteRole(Guid roleId, string token);
        Task<ResponseViewModel<bool>> ToggleRoleActiveStatus(Guid roleId, string token);
        Task<ResponseViewModel<bool>> AddRole(AddRoleViewModel addRoleViewModel, string token);
        Task<ResponseViewModel<bool>> UpdateRole(UpdateRoleViewModel updateUserViewModel, string token);
    }
}
