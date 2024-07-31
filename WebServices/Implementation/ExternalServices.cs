﻿using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Response;
using PelicanManagementUi.ViewComponents.Common.Auth;
using PelicanManagementUi.WebServices.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using PelicanManagementUi.Models.ViewModels.User;
using PelicanManagementUi.Models.ViewModels.Role;
using Microsoft.AspNetCore.DataProtection;
using System.Net.Http.Headers;
using System.Net;
using PelicanManagementUi.Models.ViewModels.Common;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using NuGet.Common;
using PelicanManagementUi.Models.ViewModels.Permission;




namespace PelicanManagementUi.WebServices.Implementation
{
    public class ExternalServices : IExternalServices
    {
        private readonly IConfiguration _configuration;
        private readonly string? serviceAddress;
        private readonly JwtTokenHandler jwtTokenHandler;


        #region User Services
        public ExternalServices(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceAddress = configuration.GetValue<string>("ServiceUrlAddress");
            this.jwtTokenHandler = new JwtTokenHandler();

        }
        public async Task<ResponseViewModel<UserAuthenticateViewModel>> Authenticate(AuthenticateViewModel viewModel)
        {
            try
            {
                if (serviceAddress == null)
                {
                    return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "کانفیگ های ارتباطی در اپ ستینگ یافت نشد." };
                }
                var requestBody = new AuthenticateViewModel { Input = viewModel.Input, Password = viewModel.Password };
                var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                var requestBytes = Encoding.UTF8.GetBytes(jsonRequestBody);

                using (var httpClient = new HttpClient())
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, serviceAddress + "/authenticate")
                    {
                        Content = new ByteArrayContent(requestBytes)
                    };
                    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");

                    var response = await httpClient.SendAsync(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<UserAuthenticateViewModel>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.SuccessLogin };
                        }
                        return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = false, Message = ErrorsMessages.FaildLogin, Status = "Api Response Status Code Is Not 200" };
                    }
                    else
                    {
                        return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = false, Message = ErrorsMessages.FaildLogin, Status = "Api Response Status Code Is Not 200" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
            }
        }
        public async Task<ResponseViewModel<List<UsersListViewModel>>> GetUserList(PaginationViewModel model, string token)
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

                    var url = $"{serviceAddress}/user/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<UsersListViewModel>>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<UsersListViewModel>> { IsSuccessFull = true,Data=responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" ,TotalCount = responseDto.TotalCount };
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
        public async Task<ResponseViewModel<UserDetailViewModel>> GetUser(Guid userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/user/get";
                    var requestBody = new GetByIdViewModel { TargetId = userId };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<UserDetailViewModel>>(responseBody);
                        return new ResponseViewModel<UserDetailViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    else
                    {
                        return new ResponseViewModel<UserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<UserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<bool>> DeleteUser(Guid userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/user/delete";
                    var requestBody = new GetByIdViewModel { TargetId = userId };
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<bool>> ToggleActiveStatus(Guid userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/user/toggle-active-status";
                    var requestBody = new GetByIdViewModel { TargetId = userId };
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<bool>> UpdateUser(UpdateUserViewModel updateUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/user/update?userId={updateUserViewModel.UserId}";
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<bool>> AddUser(AddUserViewModel addUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/user/add";
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        #endregion



        #region Role Services
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
                            return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = true,Data=responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    else
                    {
                        return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                    else
                    {
                        return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<PermissionsViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        public async Task<ResponseViewModel<GetRoleMenuViewModel>> GetRoleMenu(Guid roleId, string token)
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
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<GetRoleMenuViewModel>>(responseBody);


                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<GetRoleMenuViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                        }
                        return new ResponseViewModel<GetRoleMenuViewModel> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    else
                    {
                        return new ResponseViewModel<GetRoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }

                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<GetRoleMenuViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
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
                    else
                    {
                        return new ResponseViewModel<List<RolesListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                    var url = $"{serviceAddress}/role/update?userId={updateUserViewModel.RoleId}";
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
                    else
                    {
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }
        #endregion
    }
}
