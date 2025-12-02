using MedicalProject.Models.Product.DTOs;

namespace MedicalProject.Models.SiteEntity
{
    public class MainPageDto
    {
        public List<ProductMainPageQuery> product { get; set; } = new();
    }
    public class ProductMainPageQuery : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SeoData seoData { get; set; }
        public InventoryDto Inventory { get; set; }
        public int DangRemains { get; set; }
    }
}
