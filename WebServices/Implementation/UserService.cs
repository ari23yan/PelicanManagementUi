using Newtonsoft.Json;
using PelicanManagementUi.ViewModels.Common.Auth;
using PelicanManagementUi.ViewModels.Common.Pagination;
using PelicanManagementUi.ViewModels.Common.Response;
using PelicanManagementUi.ViewModels.Common;
using PelicanManagementUi.ViewModels.User;
using PelicanManagementUi.ViewModels;
using PelicanManagementUi.WebServices.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using PelicanManagementUi.ViewModels.Account;
using NuGet.Common;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PelicanManagementUi.ViewModels.UserActivity;

namespace PelicanManagementUi.WebServices.Implementation
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string? serviceAddress;
        private readonly JwtTokenHandler jwtTokenHandler;

        public UserService(IConfiguration configuration)
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
                        return new ResponseViewModel<UserAuthenticateViewModel> { IsSuccessFull = false, Message = responseDto.Message, Status = "Api Response Status Code Is Not 200" };
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
                        return new ResponseViewModel<UserDetailViewModel> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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
                        return new ResponseViewModel<bool> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
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

        public async Task<ResponseViewModel<bool>> ChangePassword(string password, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var url = $"{serviceAddress}/change-password";
                    var requestBody = password;
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

        public async Task<ResponseViewModel<bool>> SendSmsForChangePassword(string phoneNumber)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = $"{serviceAddress}/forget-password";
                    var requestBody = new ForgetPasswordPhoneNumberViewModel { PhoneNumber = phoneNumber};
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<bool>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<bool> { IsSuccessFull = true, Message = ErrorsMessages.Success, Status = phoneNumber };
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

        public async Task<ResponseViewModel<bool>> ConfirmOtp(ConfrimOtpViewModel viewModel)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = $"{serviceAddress}/confirm-otp";
                    var requestBody = viewModel;
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<bool>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<bool> { IsSuccessFull = true, Message = ErrorsMessages.Success, Status = viewModel.PhoneNumber };
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

        public async Task<ResponseViewModel<bool>> SubmitPasswod(ForgetPasswordViewModel viewModel)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = $"{serviceAddress}/forget-password";
                    var requestBody = viewModel;
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

        public async Task<ResponseViewModel<List<UserActivityViewModel>>> GetUserActivitesList(PaginationViewModel model, string token)
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

                    var url = $"{serviceAddress}/log/list?" + string.Join("&", queryParams);

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<List<UserActivityViewModel>>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            return new ResponseViewModel<List<UserActivityViewModel>> { IsSuccessFull = true, Data = responseDto.Data, Message = ErrorsMessages.Success, Status = "SuccessFul", TotalCount = responseDto.TotalCount };
                        }
                        return new ResponseViewModel<List<UserActivityViewModel>> { IsSuccessFull = false, Message = responseDto.Message, Status = "Failed" };
                    }
                    else
                    {
                        return new ResponseViewModel<List<UserActivityViewModel>> { IsSuccessFull = false, Message = ErrorsMessages.PermissionDenied, Status = "Api Response Status Code Is Not 200" };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel<List<UserActivityViewModel>> { IsSuccessFull = false, Message = $"{ErrorsMessages.InternalServerError}: {ex.Message}", Status = "Exception" };
                }
            }
        }
    }
}
