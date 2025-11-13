using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.Product;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front.Projects
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _service;
        private readonly IPurchaseReportService _purchaseService;

        public DetailsModel(IProductService service, IPurchaseReportService purchaseService)
        {
            _service = service;
            _purchaseService = purchaseService;
        }

        public ProductDto? Product { get; set; }
        public PurchaseReportFilterResult? purchaseReport { get; set; }
        public async Task OnGet(string slug, CancellationToken cancellationToken)
        {
            Product = await _service.GetBySlug(slug);
            purchaseReport = await _purchaseService.GetFilterForAdmin(new PurchaseReportFilterParam
            {
                ProductId = Product.Id,
            }, cancellationToken);
        }
    }
}
