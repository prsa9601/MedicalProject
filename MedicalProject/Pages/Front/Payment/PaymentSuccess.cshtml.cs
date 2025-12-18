using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Front.Payment
{
    public class PaymentSuccessModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public ProductDto Product { get; set; } = new ProductDto();

        [BindProperty(SupportsGet = true)]
        public OrderDto? Order { get; set; } = new();

        public void OnGet()
        {
        }
    }
}
