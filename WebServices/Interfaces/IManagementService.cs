using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Role;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IManagementService
    {
        Task<ResponseViewModel<List<RolesListViewModel>>> GetUsersList(PaginationViewModel model, string token);

    }
}
