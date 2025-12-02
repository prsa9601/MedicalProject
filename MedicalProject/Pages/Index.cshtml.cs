using MedicalProject.Models.SiteEntity;
using MedicalProject.Services.SiteEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISiteEntityService _service;

        public IndexModel(ILogger<IndexModel> logger, ISiteEntityService service)
        {
            _logger = logger;
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public MainPageDto? Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetMainPage();
        }
    }
}
