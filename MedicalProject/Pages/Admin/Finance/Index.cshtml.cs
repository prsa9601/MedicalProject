using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Finance
{
    public class IndexModel : BaseRazorFilter<PurchaseReportFilterParam>
    {
        private readonly IPurchaseReportService _service;

        public IndexModel(IPurchaseReportService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public PurchaseReportFilterResult? Result { get; set; }
        
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Result = await _service.GetFilterForAdmin(FilterParams, cancellationToken);
        }
    }
}
