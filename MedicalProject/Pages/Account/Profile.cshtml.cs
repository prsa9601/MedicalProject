using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _service;

        public ProfileModel(IUserService service)
        {
            _service = service;
        }
        [BindProperty(SupportsGet = true)]
        public UserDto? user { get; set; }
        public async Task OnGet()
        {
            user = await _service.GetCurrentUser();
        }
        public async Task<IActionResult> OnPostSendUserDocument(IFormFile nationalCardImage
            , IFormFile birthCertificationImage, string nationalityCode)
        {
            var result = await _service.CompletionOfInformation(new Models.User.CompletionOfInformationCommandViewModel
            {
                birthCertificatePhoto = birthCertificationImage,
                nationalCardPhoto = nationalCardImage,
                nationalityCode = nationalityCode,
            });
            if (result.IsSuccess)
            {
                TempData["Success"] = result.MetaData.Message;
                user = await _service.GetCurrentUser();
            }
            else
                TempData["Error"] = result.MetaData.Message;

            return Redirect("/Account/Profile");
        }
    }
}
