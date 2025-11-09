using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Product;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Product
{
    [BindProperties(SupportsGet = true)]
    public class EditModel : BaseRazorPage
    {
        private readonly IProductService _service;

        public EditModel(IProductService service)
        {
            _service = service;
        }

        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public ProductStatus status { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public bool IndexPage { get; set; }
        public IFormFile? productImage { get; set; }
        public string? productImageName { get; set; }
        public Guid ProductId { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }


        public async Task<IActionResult> OnGet(Guid productId)
        {
            var product = await _service.GetById(productId);
            if (product == null)
            {
                return RedirectAndShowAlert(new Models.ApiResult
                {
                    IsReload = false,
                    MetaData = new Models.MetaData
                    {
                        AppStatusCode = Models.AppStatusCode.NotFound,
                        Message = "?????? ???? ???"
                    },
                    IsSuccess = false
                }, Redirect("Index"));
            }

            title = product.Title;
            description = product.Description;
            Schema = product.SeoData.Schema;
            productImageName = product.ImageName;
            IndexPage = product.SeoData.IndexPage;
            MetaDescription = product.SeoData.MetaDescription;
            MetaKeyWords = product.SeoData.MetaKeyWords;
            MetaTitle = product.SeoData.MetaTitle;
            status = product.Status;
            slug = product.Slug;
            Canonical = product.SeoData.Canonical;
            ProductId = productId;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _service.Edit(new EditProductCommand
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
                Image = productImage,
                productId = ProductId,
            });
            return RedirectAndShowAlert(result, Redirect("Index"));
        }
    }
}
