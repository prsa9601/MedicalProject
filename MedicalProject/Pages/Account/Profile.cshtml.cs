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

        public UserDto? user { get; set; }
        public async Task OnGet()
        {
            user = await _service.GetCurrentUser();
        }
    }
}
