using MedicalProject.Models;
using MedicalProject.Models.Auth;

namespace MedicalProject.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _accessor;
        public AuthService(HttpClient client, IHttpContextAccessor accessor)
        {
            _client = client;
            _accessor = accessor;
        }

        public async Task<ApiResult<LoginCommandResult>?> Login(LoginCommand command)
        {
            var result = await _client.PostAsJsonAsync("Auth/LoginUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult<LoginCommandResult>>();
        }

        public async Task<ApiResult?> Register(RegisterCommand command)
        {
            var result = await _client.PostAsJsonAsync($"Auth/RegisterUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult?> Logout()
        {
            try
            {
                var result = await _client.DeleteAsync("auth/logout");
                return await result.Content.ReadFromJsonAsync<ApiResult>();
            }
            catch (Exception e)
            {
                return ApiResult.Error();
            }
        }

        public async Task<ApiResult<string>> GenerateAndSendOtpCode(GenerateAndSendOtpCodeCommand command)
        {
            var result = await _client.PostAsJsonAsync("Auth/GenerateOtpCode", command);
            return await result.Content.ReadFromJsonAsync<ApiResult<string>>();
        }
        public async Task<ApiResult<bool>?> VerificationOtpCode(VerificationOtpCodeCommand command)
        {
            var result = await _client.PostAsJsonAsync("Auth/VerificationOtpCode", command);
            return await result.Content.ReadFromJsonAsync<ApiResult<bool>>();
        }
    }
}
