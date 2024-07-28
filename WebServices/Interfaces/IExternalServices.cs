﻿using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.Models.ViewModels.Common.Response;
using PelicanManagementUi.Models.ViewModels.Role;
using PelicanManagementUi.Models.ViewModels.User;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IExternalServices
    {
        Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel);
        Task<ResponseViewModel<GetRoleMenuViewModel>> GetRoleMenu(Guid roleId,string token);
        Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token);
    }
}
