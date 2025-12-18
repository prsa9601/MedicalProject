using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Order;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front.Payment
{
    public class SuccessPaymentModel : PageModel
    {
        private readonly IOrderService _service;
        private readonly IProductService _productService;

        public SuccessPaymentModel(IOrderService service, IProductService productService)
        {
            _service = service;
            _productService = productService;
        }

        [BindProperty(SupportsGet = true)]
        public ProductDto Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public OrderDto? Order { get; set; }

        public async Task<IActionResult> OnGet(Guid orderId)
        {
            Order = await _service.GetById(orderId, CancellationToken.None);
            Product = await _productService.GetById(Order.OrderItems.ProductId);
            return Page();
        }
    }
}
