using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front
{
    [Authorize]
    public class ProjectsModel : PageModel
    {
        private readonly IProductService _service;

        public ProjectsModel(IProductService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public ProductFilterResult Result { get; set; }


        public async Task<IActionResult> OnGet(int pageId = 1, int take = 8)
        {
            Result = await _service.GetFilter(new Models.Product.DTOs.ProductFilterParam
            {
                Status = Models.Product.ProductStatus.IsActive,
                PageId = pageId,
                Take = take
            });
            return Page();
        }
    }
}
