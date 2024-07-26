using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Response;

namespace PelicanManagementUi.WebServices.Interfaces
{
    public interface IExternalServices
    {
        Task<ResponseViewModel<string>> Authenticate(AuthenticateViewModel viewModel);
    }
}
