using MedicalProject.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IAuthService _service;

        public LogoutModel(IAuthService service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (refreshToken != null)
            {
                _service.Logout(refreshToken);
            }
            Response.Cookies.Delete("auth-Token");
            Response.Cookies.Delete("RefreshToken");
            TempData["Success"] = "خروج از حساب کاربری با موفقیت انجام شد.";
            return Redirect("/Index");
        }
    }
}
