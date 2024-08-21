using Newtonsoft.Json;
using PelicanManagementUi.Enums;
using PelicanManagementUi.ViewModels.Common;
using PelicanManagementUi.ViewModels.Common.Auth;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Management;
using PelicanManagementUi.ViewModels.Role;
using PelicanManagementUi.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

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

        public async Task<ResponseViewModel<bool>> AddUser(AddIdentityUserViewModel addUserViewModel, string token, UserType type)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/identity/add?type={(int)type}";
                    var requestBody = addUserViewModel;

                   

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<bool>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<bool> { IsSuccessFull = true, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<bool>> DeleteUser(int userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/identity/delete";
                    var requestBody = new  { UserId = userId };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Delete, url)
                    {
                        Content = jsonContent
                    };
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<bool>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<bool> { IsSuccessFull = true, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                  
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<PermissionAndUnitsViewModel>> GetPermissionsAndUnits(string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/get-permissons-and-units-list";
                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<PermissionAndUnitsViewModel>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<PermissionAndUnitsViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<PermissionAndUnitsViewModel> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    else
                    {
                        return new ResponseViewModel<PermissionAndUnitsViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<PermissionAndUnitsViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<ViewModels.Management.UsersListViewModel>> GetTeriageUser(string username, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/identity/get-teriage-user";
                    var requestBody = new { Username = username };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<ViewModels.Management.UsersListViewModel>>(responseBody);
                        return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<ViewModels.Management.UsersListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<IdentityUserDetailViewModel>> GetUser(string username, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/identity/get";
                    var requestBody = new  { Username = username };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<IdentityUserDetailViewModel>>(responseBody);
                        return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<IdentityUserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>> GetUsersList(
            PaginationViewModel model,
            string token,
            UserType type)
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

                    queryParams.Add($"type={(int)type}");
                    var url = $"{serviceAddress}/management/identity/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>>(responseBody);

                        if (responseDto != null && responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>
                            {
                                IsSuccessFull = true,
                                Data = responseDto.Data,
                                Message = ErrorsMessages.Success,
                                Status = "SuccessFul",
                                TotalCount = responseDto.TotalCount
                            };
                        }

                        return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>
                        {
                            IsSuccessFull = false,
                            Message = responseDto?.Message ?? "Unknown error",
                            Status = "Failed"
                        };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>
                    {
                        IsSuccessFull = false,
                        Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}",
                        Status = "Exception"
                    };
                }
            }
        }

        public async Task<ResponseViewModel<bool>> UpdateUser(UpdatePelicanUserViewModel updatePelicanUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/identity/update?userId={updatePelicanUserViewModel.UserId}";
                    var requestBody = updatePelicanUserViewModel;
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Put, url)
                    {
                        Content = jsonContent
                    };
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<bool>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<bool> { IsSuccessFull = true, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
    }
}
