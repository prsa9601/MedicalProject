using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Documents
{
    public class DetailsModel : BaseRazorFilter<UserFilterForDocumentsParam>
    {
        private readonly IUserService _service;

        public DetailsModel(IUserService service)
        {
            _service = service;
        }
        [BindProperty(SupportsGet = true)]
        public UserDto User { get; set; }
        public async Task OnGet(Guid id)
        {
            User = await _service.GetUserById(id);
        }
    }
}
