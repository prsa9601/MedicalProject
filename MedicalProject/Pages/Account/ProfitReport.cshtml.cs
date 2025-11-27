using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Account
{
    public class ProfitReportModel : BaseRazorFilter<UserPurchaseReportFilterParam>
    {
        private readonly IPurchaseReportService _service;

        public ProfitReportModel(IPurchaseReportService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public UserPurchaseReportDto Result { get; set; } = new();
        public async Task<IActionResult> OnGet()
        {
            Result = await _service.GetForCurrentUser();
            return Page();
        }
    }
}
