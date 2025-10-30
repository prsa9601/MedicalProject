using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Product;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Product
{
    //[Area("Admin")]
    [BindProperties]
    public class AddModel : BaseRazorPage
    {
        private readonly IProductService _service;

        public AddModel(IProductService service)
        {
            _service = service;
        }

        public required string slug { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public ProductStatus status { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public bool IndexPage { get; set; }
        public IFormFile? productImage { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _service.Create(new CreateProductCommand
            {
                description = description,
                MetaDescription = MetaDescription,
                Canonical = Canonical,
                IndexPage = IndexPage,
                MetaKeyWords = MetaKeyWords,
                slug = slug,
                MetaTitle = MetaTitle,
                Schema = Schema,
                title = title,
                status = status,
                Image = productImage
            });
            return RedirectAndShowAlert(result, Redirect("Index"));
        }
    }
}
