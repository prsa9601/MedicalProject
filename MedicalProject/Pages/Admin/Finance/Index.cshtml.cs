using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Finance
{
    public class IndexModel : BaseRazorFilter<UserPurchaseReportFilterParam>
    {
        private readonly IPurchaseReportService _service;

        public IndexModel(IPurchaseReportService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public PurchaseReportFilterResult? Result { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public PurchaseReportUserInvestmentFilterResult? UserPurchaseReportResult { get; set; }


        [BindProperty(SupportsGet = true)]
        public UserProfitPurchaseReportDtoFilterResult ResultDto { get; set; }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            Result = await _service.GetFilterForAdmin(new PurchaseReportFilterParam
            {
                EndDate = FilterParams.EndDate,
                StartDate = FilterParams.StartDate,
                PageId = FilterParams.PageId,
                Take = FilterParams.Take,
                PhoneNumber = FilterParams.PhoneNumber,
                ProductId = FilterParams.ProductId,
                PurchaseReportFilter = FilterParams.PurchaseReportFilter,
                    
            }, cancellationToken);

            ResultDto = await _service.GetProfitFilter(new UserProfitPurchaseReportDtoFilterParam
            {
                Take = FilterParams.Take,
                PageId = FilterParams.PageId,
                //search = FilterParams.
            });
            UserPurchaseReportResult = await _service.GetFilterUserForAdmin(FilterParams, cancellationToken);
        }
    }
}
