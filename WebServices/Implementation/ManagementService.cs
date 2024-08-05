using Newtonsoft.Json;
using PelicanManagementUi.ViewModels.Common.Auth;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Management;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.WebServices.Interfaces;
using System.Net.Http.Headers;

namespace PelicanManagementUi.WebServices.Implementation
{
    public class ManagementService: IManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly string? serviceAddress;
        private readonly JwtTokenHandler jwtTokenHandler;
        public ManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceAddress = configuration.GetValue<string>("ServiceUrlAddress");
            this.jwtTokenHandler = new JwtTokenHandler();
        }


        public async Task<ResponseViewModel<List<UsersListViewModel>>> GetUsersList(PaginationViewModel model, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var queryParams = new List<string>
                    {
                        $"pageNumber={model.PageNumber}",
                        $"pageSize={model.PageSize}"
                    };

                    if (!string.IsNullOrEmpty(model.Searchkey))
                    {
                        queryParams.Add($"searchkey={Uri.EscapeDataString(model.Searchkey)}");
                    }

                    if (model.FilterType.HasValue)
                    {
                        queryParams.Add($"filterType={model.FilterType.Value}");
                    }

                    var url = $"{serviceAddress}/management/identity/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<UsersListViewModel>>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<UsersListViewModel>> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul", TotalCount = responseDto.TotalCount };
                        }
                        return new ResponseViewModel<List<UsersListViewModel>> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    else
                    {
                        return new ResponseViewModel<List<UsersListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<UsersListViewModel>> { IsSuccessFull = false, Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}", Status = "Exception" };
                }
            }
        }
    }
}
