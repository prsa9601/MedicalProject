using MedicalProject.Models;
using MedicalProject.Models.Auth;

namespace MedicalProject.Services.Auth
{
    public interface IAuthService
    {
        //Task<ApiResult<LoginResponse>?> Login(LoginCommand command);
        Task<ApiResult<string>?> Login(LoginCommand command);
        Task<ApiResult?> Register(RegisterCommand command);
        Task<ApiResult?> GenerateAndSendOtpCode(GenerateAndSendOtpCodeCommand command);
        Task<ApiResult?> VerificationOtpCode(VerificationOtpCodeCommand command);

        Task<ApiResult?> Logout();
    }
}
