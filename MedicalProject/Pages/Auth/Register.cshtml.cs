using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Auth
{
    [BindProperties]
    public class RegisterModel : BaseRazorPage
    {
        private readonly IAuthService _service;

        public RegisterModel(IAuthService service)
        {
            _service = service;
        }

        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        [Display(Name = "رمزعبور")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمزعبور")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        public void OnGet(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public async Task<IActionResult> OnPost()
        {
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;

            if (Password == ConfirmPassword)
            {
                var result = await _service.Register(new Models.Auth.RegisterCommand
                {
                    firstName = FirstName,
                    lastName = LastName,
                    password = Password,
                    phoneNumber = PhoneNumber,
                    ipAddress = ipAddress ?? "0000-0000-0000-0000",
                });

                if (result!.IsSuccess)
                {
                    TempData["Success"] = result.MetaData.Message;
                    return RedirectAndShowAlert(result, Redirect("/auth/Login"));
                }
                else
                {
                    TempData["Error"] = result.MetaData.Message;
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(ConfirmPassword, "تکرار رمزعبور و رمز عبور مطابقت ندارند.");
            }


            var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

            TempData["Error"] = string.Join(" | ", errors);
            return Page();
        }
    }
}
