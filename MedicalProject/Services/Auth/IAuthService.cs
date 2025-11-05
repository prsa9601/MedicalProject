using MedicalProject.Models;
using MedicalProject.Models.Auth;

namespace MedicalProject.Services.Auth
{
    public interface IAuthService
    {
        //Task<ApiResult<LoginResponse>?> Login(LoginCommand command);
        Task<ApiResult<LoginCommandResult>?> Login(LoginCommand command);
        Task<ApiResult?> Register(RegisterCommand command);
        Task<ApiResult<string>> GenerateAndSendOtpCode(GenerateAndSendOtpCodeCommand command);
        Task<ApiResult<bool>?> VerificationOtpCode(VerificationOtpCodeCommand command);

        Task<ApiResult?> Logout();
    }
}
