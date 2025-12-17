using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Contact;
using MedicalProject.Services.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedicalProject.Pages.Admin.Contact
{
    public class IndexModel : BaseRazorFilter<ContactFilterParam>
    {
        private readonly IContactService _service;

        public IndexModel(IContactService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public ContactFilterResult Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetFilter(FilterParams);
        }

        public async Task<IActionResult> OnPostContactDetails(Guid id)
        {
            Result = await _service.GetFilter(FilterParams);

            var result = Result.Data.FirstOrDefault(i => i.Id.Equals(id));
            return new JsonResult(new
            {
                phoneNumber = result.PhoneNumber,
                fullName = result.FullName,
                email = result.Email,
                title = result.Title,
                description = result.Description,
                creationDate = result.CreationDate,
                contactId = result.Id,
            });
        }

        public async Task<IActionResult> OnPostAnswered(Guid id)
        {
            Result = await _service.GetFilter(FilterParams);

            var result = await _service.Answered(new ContactAnsweredCommand { id = id });
            if (result.IsSuccess)
                return Page();

            TempData["Error"] = result.MetaData.Message;
            return Page();
        }
    }
}