using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front.Payment
{
    public class InvoiceModel : PageModel
    {
        private readonly IProductService _service;

        public InvoiceModel(IProductService productService)
        {
            _service = productService;
        }

        [BindProperty(SupportsGet = true)]
        public ProductDto Product { get; set; }

        public async Task OnGet(string slug)
        {
            Product = await _service.GetBySlug(slug);
        }
    }
}
