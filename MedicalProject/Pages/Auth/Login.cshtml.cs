using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Auth;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;

        public LoginModel(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [BindProperty]
        public string RedirectTo { get; set; }

        [BindProperty]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }


        [BindProperty]
        public UserDto user { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {

            if (string.IsNullOrEmpty(PhoneNumber))
            {
                ModelState.AddModelError(PhoneNumber, "لطفا نام کاربری خود را واردکنید.");
                return Page();
            }

            if (string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(Password, "لطفا رمز عبور خود را واردکنید.");
                return Page();
            }



            try
            {
                var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? forwardedHeader;

                var result = await _service.Login(new Models.Auth.LoginCommand
                {
                    ipAddress = ipAddress ?? "0000-0000-0000-0000",
                    password = Password,
                    phoneNumber = PhoneNumber,
                    rememberMe = RememberMe
                });

                if (result!.IsSuccess)
                {
                    //Response.Cookies.Append("auth-Token", result!.Data, new CookieOptions
                    //{
                    //    Expires = DateTimeOffset.Now.AddMinutes(30),
                    //    HttpOnly = true,
                    //    IsEssential = true
                    //});
                    //Response.Cookies.Append("auth-Token", result!.Data, new CookieOptions
                    //{
                    //    Expires = RememberMe ? DateTimeOffset.Now.AddDays(30) : DateTimeOffset.Now.AddHours(24),
                    //    HttpOnly = true,
                    //    IsEssential = true
                    //});

                    if (result.Data.AuthToken is not null)
                    {
                        Response.Cookies.Append("auth-Token", result!.Data.AuthToken, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddMinutes(30),
                            HttpOnly = true,
                            IsEssential = true
                        });
                    }
                    if (result.Data.RefreshToken is not null)
                    {
                        Response.Cookies.Append("RefreshToken", result!.Data.RefreshToken, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(14),
                            HttpOnly = true,
                            IsEssential = true
                        });
                    }


                    TempData["Success"] = result.MetaData.Message;
                    return RedirectToPage($"{RedirectTo}");

                }

                TempData["Error"] = result.MetaData.Message;
                return Page();
            }
            catch (Exception ex)
            {
                //ErrorMessage = "??? ?? ???? ?? ?????. ????? ?????? ???? ????.";

                return Page();
            }
        }

        //private async Task<bool> AuthenticateUser(string username, string password)
        //{
        //    // ????? ???? ???? ????? ????? ???? ?????????? ???
        //    // ???? ?????? ????? ?? ???????
        //    await Task.Delay(100); // ????????? ????? ????

        //    // ????? ???? - ?? ???? ????? ???? ?? ??????? ?? ???
        //    var validUsers = new Dictionary<string, string>
        //    {
        //        { "admin", "admin123" },
        //        { "user", "user123" }
        //    };

        //    return validUsers.Any(u => u.Key == username && u.Value == password);
        //}
    }
}
