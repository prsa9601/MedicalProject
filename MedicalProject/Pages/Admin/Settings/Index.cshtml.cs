using MedicalProject.Services.SiteSetting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Settings
{
    public class IndexModel : PageModel
    {
        private readonly ISiteSettingService _service;

        public IndexModel(ISiteSettingService service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }
        public async Task OnPostSaveSettings(string companyName)
        {
            var result = await _service.CreateOrEdit(new Models.SiteSetting.CreateOrEditCommand
            {
                ComparyName = companyName,
            });
        }
    }
}
