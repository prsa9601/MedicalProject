using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Pages.Auth
{
    public class VerificationPhoneNumberModel : PageModel
    {


        public string PhoneNumber { get; set; }
        
        [BindProperty]        
        [Display(Name = "کد عبور یکبار مصرف")]
        public string Token { get; set; }
        public void OnGet()
        {
        }
    }
}
