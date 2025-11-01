using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Services.Auth;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
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
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "تکرار رمزعبور")]
        public string ConfirmPassword { get; set; }


        public void OnGet(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        
        public async Task<IActionResult> OnPost()
        {
            var result = await _service.ChangePassword(new Models.User.ChangePasswordCommand
            {
                password = Password,
            });

            if (result.IsSuccess)
                return RedirectAndShowAlert(result, Redirect("/Index"));

            TempData["Error"] = result.MetaData.Message;
            return Page();
        }
    }
}
