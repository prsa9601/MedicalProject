using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace MedicalProject.Pages.Auth
{
    public class VerificationPhoneNumberModelModel : PageModel
    {
        [BindProperty]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }
    } 
}
