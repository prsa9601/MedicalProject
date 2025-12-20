using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Services.Product;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Front
{
    public class ProjectsModel : BaseRazorFilter<ProductFilterParam>
    {
        private readonly IProductService _service;

        public ProjectsModel(IProductService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public ProductFilterForIndexPageResult Result { get; set; }


        public async Task<IActionResult> OnGet(int pageId = 1, int take = 8)
        {
            Result = await _service.GetFilterForIndexPage(new Models.Product.DTOs.ProductFilterParam
            {
                //Status = Models.Product.ProductStatus.IsActive,
                PageId = pageId,
                Take = take,
            });
            return Page();
        }
    }
}
