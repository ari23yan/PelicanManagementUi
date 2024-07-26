using PelicanManagementUi.Models.ViewModels;
using PelicanManagementUi.Models.ViewModels.Common.Response;
using PelicanManagementUi.ViewComponents.Common.Auth;
using PelicanManagementUi.WebServices.Interfaces;
using Microsoft.Extensions.Configuration;


namespace PelicanManagementUi.WebServices.Implementation
{
    public class ExternalServices : IExternalServices
    {
        private readonly IConfiguration _configuration;
        private readonly string? serviceAddress;
        private readonly JwtTokenHandler jwtTokenHandler;

        public ExternalServices(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceAddress = configuration.GetValue<string>("ServiceAddress");
            this.jwtTokenHandler = new JwtTokenHandler();

        }

        public async Task<ResponseViewModel<string>> Authenticate(AuthenticateViewModel viewModel)
        {
            try
            {
                if (jwtTokenHandler.IsTokenValid())
                {
                    return new ResponseViewModel<string> { IsSuccessFull = true, Data = jwtTokenHandler.Token };
                }

                if (serviceAddress.IsNullOrEmpty())
                {
                    return new ResponseViewModel<string> { IsSuccessFull = false, Message = ErrorsMessages.Faild, Status = "کانفیگ های ارتباطی در اپ ستینگ یافت نشد." };
                }
                string requestBody = $"Username={Uri.EscapeDataString(Username)}&Password={Uri.EscapeDataString(Password)}";
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestBody);

                using (var httpClient = new HttpClient())
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, middlewareAddress + "Api/Authenticate")
                    {
                        Content = new ByteArrayContent(requestBytes)
                    };
                    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseViewModel<string>>(responseBody);
                        if (responseDto.IsSuccessFull.HasValue && responseDto.IsSuccessFull.Value)
                        {
                            jwtTokenHandler.Token = responseDto.Data;
                            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                            var jwtToken = tokenHandler.ReadJwtToken(responseDto.Data);
                            var expiry = jwtToken.Claims.First(c => c.Type == "exp").Value;
                            jwtTokenHandler.ExpiryTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry)).UtcDateTime;
                        }
                        return responseDto;
                    }
                    else
                    {
                        return new ResponseViewModel<string> { IsSuccessFull = false, Data = response.StatusCode.ToString() + "  /  " + response.Content.ToString(), Message = ErrorsMessages.Faild, Status = "Api Response Status Code Is Not 200" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<string> { IsSuccessFull = false, Data = ex.Message, Message = ErrorsMessages.InternalServerError, Status = "Exception" };
            }
            return new ResponseViewModel<string>();
        }

       

    }
}
