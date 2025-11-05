using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Auth;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Auth
{
   
    public class RegisterModel : BaseRazorPage
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;

        public RegisterModel(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [BindProperty]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }
        
        [BindProperty]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "تکرار رمزعبور")]
        public string ConfirmPassword { get; set; }


        [BindProperty]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [BindProperty]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [BindProperty]
        public string RedirectTo { get; set; }

        [BindProperty]
        public UserDto? user { get; set; }

        public async Task<IActionResult> OnGet(string phoneNumber, string redirectTo)
        {
            if (phoneNumber == null)
            {
                TempData["Error"] = "ابتدا باید درخواست کد تایید بدهید.";
                return Redirect("/Auth/VerificationPhoneNumber");
            }
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;

            user = await _userService.CheckOtpCodeForPhoneNumber(phoneNumber, ipAddress);
            if (user == null)
            {
                TempData["Error"] = "ابتدا باید درخواست کد تایید بدهید.";
                return Redirect("/Auth/VerificationPhoneNumber");
            }
            RedirectTo = redirectTo ?? "/Index";
            PhoneNumber = phoneNumber;
            return Page();
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
                    phoneNumber = user.PhoneNumber,
                    ipAddress = ipAddress ?? "0000-0000-0000-0000",
                });

                if (result!.IsSuccess)
                {
                    TempData["Success"] = result.MetaData.Message;
                    return RedirectAndShowAlert(result, Redirect($"/auth/Login?phoneNumber={user.PhoneNumber}&redirectTo={RedirectTo}"));
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
