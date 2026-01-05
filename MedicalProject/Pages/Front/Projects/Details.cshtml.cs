using MedicalProject.Models;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Order;
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
        private readonly IOrderService _orderService;

        public DetailsModel(IProductService service, IPurchaseReportService purchaseService, IUserService userService, IOrderService orderService)
        {
            _service = service;
            _purchaseService = purchaseService;
            _userService = userService;
            _orderService = orderService;
        }
        [BindProperty(SupportsGet = true)]
        public ProductDto? Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserDto? UserDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public PurchaseReportFilterResult? purchaseReport { get; set; }
        public async Task OnGet(string slug, CancellationToken cancellationToken)
        {
            UserDto = await _userService.GetCurrentUser() ?? null;
            Product = await _service.GetBySlug(slug);
            purchaseReport = await _purchaseService.GetFilterForAdmin(new PurchaseReportFilterParam
            {
                ProductId = Product.Id,
            }, cancellationToken);
        }
        public async Task<IActionResult> OnPostSetProject(decimal dongAmount, string slug)
        {
            Product = await _service.GetBySlug(slug);

            var orderResult = await _orderService.SetOrderItem(new Models.Order.SetOrderItemCommandViewModel
            {
                productId = Product.Id,
                dongAmount = dongAmount
            }, CancellationToken.None);

            if (orderResult.IsSuccess)
            {
                /*?slug={Product.Slug}*/
                return Redirect($"/Front/Payment/Payment");
            }
            else
            {
                TempData["Error"] = orderResult.MetaData.Message;
                return Redirect($"/Front/Projects/Details?slug={Product.Slug}");
            }
        }
    }
}
