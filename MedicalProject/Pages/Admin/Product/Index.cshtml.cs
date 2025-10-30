using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Product
{
    public class IndexModel : BaseRazorFilter<ProductFilterParam>
    {
        private readonly IProductService _service;

        public IndexModel(IProductService service)
        {
            _service = service;
        }
        [BindProperty(SupportsGet = true)]
        public ProductFilterResult Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetFilter(FilterParams);
        }
        public async Task<IActionResult> OnPostDelete(Guid productId)
        {
            var result = await _service.Remove(productId);

            if (result.IsSuccess)
            {
                return RedirectAndShowAlert(result, RedirectToPage("Index"));
            }
            else
            {
                return RedirectAndShowAlert(result, RedirectToPage("Index"));
            }
        }
    }
}
