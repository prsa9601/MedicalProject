using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models;
using MedicalProject.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Pages.Auth
{
    public class VerificationOtpCodeModel : BaseRazorPage
    {
        private readonly IAuthService _service;

        public VerificationOtpCodeModel(IAuthService service)
        {
            _service = service;
        }

        [BindProperty]
        public string Path { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }
     
        [BindProperty]
        public ForAuthAction Action { get; set; }

        [BindProperty]
        [Display(Name = "کد عبور یکبار مصرف")]
        public string Token { get; set; }
        public async Task OnGet(string phoneNumber, string path, ForAuthAction action)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) && action == null)
            {
                TempData["Error"] = "اول برای کد تایید درخواست بدهید.";
                Redirect("VerificationPhoneNumber");
            }
            PhoneNumber = phoneNumber;
            Action = action;
            Path = path ?? "/Index";
        }
        public async Task<IActionResult> OnPost()
        {
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;

            var result = await _service.VerificationOtpCode(new Models.Auth.VerificationOtpCodeCommand
            {
                phoneNumber = PhoneNumber,
                token = Token,
                ipAddress = ipAddress ?? "0000-0000-0000-0000",
            });

            if (result!.IsSuccess)
                return RedirectAndShowAlert(new Models.ApiResult
                {
                    IsSuccess = result.IsSuccess,
                    IsReload = false,
                    MetaData = new Models.MetaData
                    {
                        AppStatusCode = result.MetaData.AppStatusCode,
                        Message = result.MetaData.Message,
                    }
                }, Redirect($"{Action}?phoneNumber={PhoneNumber}&path={Path}"));
            
            TempData["Error"] = result.MetaData.Message;

            return Page();
        }
    }
}
