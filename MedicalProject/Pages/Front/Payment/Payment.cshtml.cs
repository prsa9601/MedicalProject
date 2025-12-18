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
        
        public async Task OnGet()
        {
            Order = await _orderService.GetCurrentUser();
            Product = await _service.GetById(Order.OrderItems.ProductId);
            //    var order = await _orderService.SetOrderItem(new Models.Order.SetOrderItemCommandViewModel
            //    {
            //        productId =Product.Id,
            //        dongAmount = 
            //    });
        }
        public async Task<IActionResult> OnPost()
        {
            Order = await _orderService.GetCurrentUser();

            var result = await _orderService.IsFinally(new OrderIsFinallyViewModel
            {
                orderId = Order.Id
            }, CancellationToken.None);

            if (result.IsSuccess)
            {  // return Redirect($"/Front/Payment/SuccessPayment/{Order.Id}");
                return new JsonResult(new
                {
                    success = true,
                    redirectUrl = $"/Front/Payment/SuccessPayment/{Order.Id}"
                });
            }
            TempData["Error"] = result.MetaData.Message;
            return new JsonResult(new
            {
                success = false,
                message = result.MetaData.Message
            });

            return Redirect("/Front/Payment/Payment");
        }
    }
}
