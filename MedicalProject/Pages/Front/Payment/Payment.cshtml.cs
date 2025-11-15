using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Front.Payment
{
    public class PaymentModel : PageModel
    {
        private readonly IProductService _service;

        public PaymentModel(IProductService service)
        {
            _service = service;
        }

        public ProductDto Product { get; set; }
        public async Task OnGet()
        {
            Product = await _service.GetBySlug("222222");
        }
    }
}
