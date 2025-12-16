using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Profit;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Finance
{
    public class UserReportModel : BaseRazorPage
    {
        private readonly IPurchaseReportService _service;
        private readonly IProfitService _profitService;

        public UserReportModel(IPurchaseReportService service, IProfitService profitService)
        {
            _service = service;
            _profitService = profitService;
        }

        [BindProperty(SupportsGet = true)]
        public UserPurchaseReportDto Result { get; set; }
       
        [BindProperty(SupportsGet = true)]
        public UserProfitPurchaseReportDto ReportDto { get; set; }

        [BindProperty]
        public Guid orderId { get; set; }
        [BindProperty]
        public Guid userId { get; set; }
        [BindProperty]
        public Guid productId { get; set; }
        [BindProperty]
        public DateTime forWhaeTime { get; set; }
        [BindProperty]
        public int forWhatPeriod { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Result = await _service.GetById(id);
            ReportDto = await _service.GetProfitById(id);
            return Page();
        }
      
        public async Task<IActionResult> OnPostProfitPayed()
        {
            var result = await _profitService.Create(new Models.Profit.CreateProfitCommand
            {
                OrderId = orderId,
                UserId = userId,
                Status = ProfitStatus.Success,
                ProductId = productId,
                ForWhatePeriod = forWhatPeriod,
                ForWhateTime = forWhaeTime
            });
            if (result.IsSuccess)
            {
                Result = await _service.GetById(userId);
                ReportDto = await _service.GetProfitById(userId);
            }
            Result = await _service.GetById(userId);
            ReportDto = await _service.GetProfitById(userId);
            return Page();
        }

    }
}

