using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Account
{
    public class InvestmentModel : BaseRazorFilter<UserPurchaseReportForCurrentUserFilterParam>
    {
        private readonly IPurchaseReportService _service;

        public InvestmentModel(IPurchaseReportService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public PurchaseReportUserInvestmentFilterResult Result { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserPurchaseReportDto UserDto { get; set; }

        public async Task OnGet()
        {
            Result = await _service.GetFilterUserForCurrentUser(FilterParams);
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id);
            UserDto = Result.Data.FirstOrDefault(i => i.UserId.Equals(id));
        }
    }
}
