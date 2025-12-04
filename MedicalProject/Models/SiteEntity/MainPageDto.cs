using MedicalProject.Models.Product;
using MedicalProject.Models.Product.DTOs;

namespace MedicalProject.Models.SiteEntity
{
    public class MainPageDto
    {
        public int InvestmentNmber { get; set; }
        public List<ProductMainPageQuery> product { get; set; } = new();
    }
    public class ProductMainPageQuery : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SeoData seoData { get; set; }
        public InventoryDto Inventory { get; set; }
        public ProductStatus Status { get; set; }
        public int DangRemains { get; set; }
    }
}
