using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Auth;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Auth
{
    public class ResetPasswordModel : BaseRazorPage
    {
        private readonly IUserService _service;

        public ResetPasswordModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        public int VerificationCode { get; set; }

        [BindProperty]
        [Display(Name = "رمزعبور")]
        public string NewPassword { get; set; }

        [BindProperty]
        [Display(Name = "تکرار رمزعبور")]
        public string ConfirmPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserDto? user { get; set; }

        public async Task<IActionResult> OnGet(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                TempData["Error"] = "ابتدا باید درخواست کد تایید بدهید.";
                return Redirect("/Auth/VerificationPhoneNumber");
            }
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;

            user = await _service.CheckOtpCodeForPhoneNumber(phoneNumber, ipAddress);
            if (user.FirstName == null && user.LastName == null)
            {
                TempData["Error"] = "ابتدا باید ثبت نام کنید";
                return Redirect("/Index");
            }
            if (user == null)
            {
                TempData["Error"] = "ابتدا باید درخواست کد تایید بدهید.";
                return Redirect($"/Auth/VerificationPhoneNumber?Action={ForAuthAction.ResetPassword}");
            }
            PhoneNumber = phoneNumber;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;
            var result = await _service.ChangePassword(new Models.User.ChangePasswordCommand
            {
                ipAddress = ipAddress ?? "0000-0000-0000-0000",
                password = NewPassword,
                userId = user.Id
            });

            //برای otpcode بیاد و ارور بده سمت سرور

            if (result.IsSuccess)
            {
                TempData["Success"] = "رمز عبور با موفقیت تغییر کرد.";
                return Redirect("/Index");
            }

            TempData["Error"] = result.MetaData.Message;
            return Page();
        }
    }
}
