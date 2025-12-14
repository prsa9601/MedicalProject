using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Order;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Front.Payment
{
    public class PaymentModel : PageModel
    {
        private readonly IProductService _service;
        private readonly IOrderService _orderService;

        public PaymentModel(IProductService service, IOrderService orderService)
        {
            _service = service;
            _orderService = orderService;
        }
        
        [BindProperty(SupportsGet = true)]
        public ProductDto Product { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public OrderDto? Order { get; set; }
        
        public async Task OnGet(string slug)
        {
            Product = await _service.GetBySlug(slug);
            //    Order = await _orderService.get
            //    var order = await _orderService.SetOrderItem(new Models.Order.SetOrderItemCommandViewModel
            //    {
            //        productId =Product.Id,
            //        dongAmount = 
            //    });
        }
    }
}
