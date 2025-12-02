using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Product;
using MedicalProject.Services.PurchaseReport;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front.Projects
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _service;
        private readonly IUserService _userService;
        private readonly IPurchaseReportService _purchaseService;

        public DetailsModel(IProductService service, IPurchaseReportService purchaseService, IUserService userService)
        {
            _service = service;
            _purchaseService = purchaseService;
            _userService = userService;
        }
        [BindProperty(SupportsGet = true)]
        public ProductDto? Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserDto? UserDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public PurchaseReportFilterResult? purchaseReport { get; set; }
        public async Task OnGet(string slug, CancellationToken cancellationToken)
        {
            UserDto = await _userService.GetCurrentUser();
            Product = await _service.GetBySlug(slug);
            purchaseReport = await _purchaseService.GetFilterForAdmin(new PurchaseReportFilterParam
            {
                ProductId = Product.Id,
            }, cancellationToken);
        }
    }
}
