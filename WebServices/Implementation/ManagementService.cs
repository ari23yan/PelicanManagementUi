using Newtonsoft.Json;
using UsersManagementUi.Enums;
using UsersManagementUi.ViewModels.Common;
using UsersManagementUi.ViewModels.Common.Auth;
using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Common.Response;
using UsersManagementUi.ViewModels.Management;
using UsersManagementUi.ViewModels.Role;
using UsersManagementUi.ViewModels.User;
using UsersManagementUi.WebServices.Interfaces;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace UsersManagementUi.WebServices.Implementation
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
                    var url = $"{serviceAddress}/management/add?type={(int)type}";
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
                    var url = $"{serviceAddress}/management/delete";
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

        public async Task<ResponseViewModel<bool>> DeleteClinicUser(string userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/delete-clinic-user";
                    var requestBody = new { Username = userId };
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

        public async Task<ResponseViewModel<bool>> DeleteHisNovinUser(string userId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/delete-his-novin-user";
                    var requestBody = new { Username = userId };
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


        public async Task<ResponseViewModel<bool>> AddClinicUser(AddClinicUserViewModel addUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/add-clinic-user";
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

        public async Task<ResponseViewModel<bool>> AddHisNovinUser(AddHisNovinUserViewModel addUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/add-his-novin-user";
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
        public async Task<ResponseViewModel<ClinicUserListViewModel>> GetClinicUser(string username, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/get-clinic-user";
                    var requestBody = new { Username = username };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<ClinicUserListViewModel>>(responseBody);
                        return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<ClinicUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<List<ClinicUserListViewModel>>> GetClinicUsersList(PaginationViewModel model, string token)
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

                    queryParams.Add($"type={4}");
                    var url = $"{serviceAddress}/management/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<ClinicUserListViewModel>>>(responseBody);

                        if (responseDto != null && responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<ClinicUserListViewModel>>
                            {
                                IsSuccessFull = true,
                                Data = responseDto.Data,
                                Message = ErrorsMessages.Success,
                                Status = "SuccessFul",
                                TotalCount = responseDto.TotalCount
                            };
                        }

                        return new ResponseViewModel<List<ClinicUserListViewModel>>
                        {
                            IsSuccessFull = false,
                            Message = responseDto?.Message ?? "Unknown error",
                            Status = "Failed"
                        };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<ClinicUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<ClinicUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<ClinicUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<ClinicUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<ClinicUserListViewModel>>
                    {
                        IsSuccessFull = false,
                        Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}",
                        Status = "Exception"
                    };
                }
            }
        }

        public async Task<ResponseViewModel<HisNovinUserListViewModel>> GetHisNovinUser(string username, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/get-his-novin-user";
                    var requestBody = new { Username = username };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<HisNovinUserListViewModel>>(responseBody);
                        return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul" };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<HisNovinUserListViewModel> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
                }
            }
        }

        public async Task<ResponseViewModel<bool>> UpdateClinicUser(UpdateClinicUserViewModel updateClinicUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/update-clinic-user?userId={updateClinicUserViewModel.UserId}";
                    var requestBody = updateClinicUserViewModel;
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

        public async Task<ResponseViewModel<bool>> UpdateHisNovinUser(UpdateHisNovinUserViewModel updateHisUserViewModel, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/management/update-his-novin-user?userId={updateHisUserViewModel.UserId}";
                    var requestBody = updateHisUserViewModel;
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
        public async Task<ResponseViewModel<List<HisNovinUserListViewModel>>> GetHisNovinUsersList(PaginationViewModel model, string token)
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

                    queryParams.Add($"type={3}");
                    var url = $"{serviceAddress}/management/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<HisNovinUserListViewModel>>>(responseBody);

                        if (responseDto != null && responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<HisNovinUserListViewModel>>
                            {
                                IsSuccessFull = true,
                                Data = responseDto.Data,
                                Message = ErrorsMessages.Success,
                                Status = "SuccessFul",
                                TotalCount = responseDto.TotalCount
                            };
                        }

                        return new ResponseViewModel<List<HisNovinUserListViewModel>>
                        {
                            IsSuccessFull = false,
                            Message = responseDto?.Message ?? "Unknown error",
                            Status = "Failed"
                        };
                    }
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                            return new ResponseViewModel<List<HisNovinUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.NotAuthenticated, Status = "Unauthorized" };
                        case System.Net.HttpStatusCode.Forbidden:
                            return new ResponseViewModel<List<HisNovinUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Forbidden" };

                        case System.Net.HttpStatusCode.InternalServerError:
                            return new ResponseViewModel<List<HisNovinUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = "InternalServerError" };
                        default:
                            return new ResponseViewModel<List<HisNovinUserListViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.InternalServerError, Status = $"Api Response Status Code Is {response.StatusCode}" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<HisNovinUserListViewModel>>
                    {
                        IsSuccessFull = false,
                        Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}",
                        Status = "Exception"
                    };
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
                    var url = $"{serviceAddress}/management/get-teriage-user";
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
                    var url = $"{serviceAddress}/management/get";
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

        public async Task<ResponseViewModel<List<ViewModels.Management.UsersListViewModel>>> GetUsersList(PaginationViewModel model,string token,UserType type)
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
                    var url = $"{serviceAddress}/management/list?" + string.Join("&", queryParams);

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
                    var url = $"{serviceAddress}/management/update?userId={updatePelicanUserViewModel.UserId}";
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
    }
}
