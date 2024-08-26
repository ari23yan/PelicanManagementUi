using Newtonsoft.Json;
using UsersManagementUi.ViewModels.Common;
using UsersManagementUi.ViewModels.Common.Auth;
using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Common.Response;
using UsersManagementUi.ViewModels.Permission;
using UsersManagementUi.ViewModels.Role;
using UsersManagementUi.WebServices.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace UsersManagementUi.WebServices.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _configuration;
        private readonly string? serviceAddress;
        private readonly JwtTokenHandler jwtTokenHandler;
        public RoleService(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceAddress = configuration.GetValue<string>("ServiceUrlAddress");
            this.jwtTokenHandler = new JwtTokenHandler();
        }

        public async Task<ResponseViewModel<List<RolesListViewModel>>> GetRolesList(string token)
        {

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/get-list";
                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<RolesListViewModel>>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<List<PermissionsViewModel>>> GetRolePermissions(Guid roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/get-role-permissions";
                    var requestBody = new GetByIdViewModel { TargetId = roleId };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<PermissionsViewModel>>>(responseBody);
                        return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<RolesListWithPermissionAndMenusViewModel>> GetRolePermissionsAndMenus(Guid? roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/get-permissons-and-menus-list?TargetId={roleId}";
                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<RolesListWithPermissionAndMenusViewModel>>(responseBody);
                        return new ResponseViewModel<RolesListWithPermissionAndMenusViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    else
                    {
                        return new ResponseViewModel<RolesListWithPermissionAndMenusViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<RolesListWithPermissionAndMenusViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<RoleMenuViewModel>> GetRoleMenu(Guid roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/get-role-menu";
                    var requestBody = new GetByIdViewModel { TargetId = roleId };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<RoleMenuViewModel>>(responseBody);


                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }

                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<List<RolesListViewModel>>> GetRoleList(PaginationViewModel model, string token)
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

                    var url = $"{serviceAddress}/role/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<RolesListViewModel>>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul", TotalCount = responseDto.TotalCount };
                        }
                        return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}", Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<bool>> DeleteRole(Guid roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/delete";
                    var requestBody = new GetByIdViewModel { TargetId = roleId };
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
        public async Task<ResponseViewModel<bool>> ToggleRoleActiveStatus(Guid roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/toggle-active-status";
                    var requestBody = new GetByIdViewModel { TargetId = roleId };
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
        public async Task<ResponseViewModel<bool>> AddRole(AddRoleViewModel addRoleViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/add";
                    var requestBody = addRoleViewModel;
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
        public async Task<ResponseViewModel<bool>> UpdateRole(UpdateRoleViewModel updateUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/update?roleId={updateUserViewModel.RoleId}";
                    var requestBody = updateUserViewModel;
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
                        default:
                            var errorMessage = await response.Content.ReadAsStringAsync();
                            return new ResponseViewModel<bool> { IsSuccessFull = false, Message = errorMessage, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<RoleMenuViewModel>> GetRole(Guid roleId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/role/get";
                    var requestBody = new GetByIdViewModel { TargetId = roleId };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<RoleMenuViewModel>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }

                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<RoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
    }
}
