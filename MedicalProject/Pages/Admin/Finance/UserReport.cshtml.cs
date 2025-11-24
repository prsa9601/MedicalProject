using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Finance
{
    public class UserReportModel : BaseRazorPage
    {
        private readonly IPurchaseReportService _service;

        public UserReportModel(IPurchaseReportService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public UserPurchaseReportDto Result { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Result = await _service.GetById(id);

            return Page();
        }

    }
}

