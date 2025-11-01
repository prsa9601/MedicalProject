using MedicalProject.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _service;

        public LoginModel(IAuthService service)
        {
            _service = service;
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


        public void OnGet(string phoneNumber, string redirectTo)
        {
            RedirectTo = redirectTo ?? "/Index";
            PhoneNumber = phoneNumber;
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
                    Response.Cookies.Append("auth-Token", result!.Data, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddMinutes(30),
                        HttpOnly = true,
                        IsEssential = true
                    });
                    //Response.Cookies.Append("auth-Token", result!.Data, new CookieOptions
                    //{
                    //    Expires = RememberMe ? DateTimeOffset.Now.AddDays(30) : DateTimeOffset.Now.AddHours(24),
                    //    HttpOnly = true,
                    //    IsEssential = true
                    //});

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
