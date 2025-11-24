using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models;
using MedicalProject.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Auth
{
    public class VerificationPhoneNumberModel : BaseRazorPage
    {
        private readonly IAuthService _service;

        public VerificationPhoneNumberModel(IAuthService service)
        {
            _service = service;
        }


        [BindProperty]
        public ForAuthAction Action { get; set; }

        [BindProperty]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }
        public void OnGet(ForAuthAction action)
        {
            if (action == null)
                Action = ForAuthAction.Login;
            else
                Action = action;
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _service.GenerateAndSendOtpCode(new Models.Auth.GenerateAndSendOtpCodeCommand
            {
                phoneNumber = PhoneNumber,
            });
            if (result.IsSuccess)
            {
                if (result.Data.ContainsValue("Register"))
                {
                    TempData["Success"] = "کد تایید برای شما ارسال شد." +
                        "ابتدا ثبت نام کنید.";
                    return Redirect($"VerificationOtpCode?phoneNumber={PhoneNumber}&action={ForAuthAction.Register}");
                }
                TempData["Success"] = "کد تایید برای شما ارسال شد.";
                return Redirect($"VerificationOtpCode?phoneNumber={PhoneNumber}&action={Action}");
            }

            TempData["Error"] = result.MetaData.Message;

            return Page();
        }
    }
}
