using MedicalProject.Models.PurchaseReport;

namespace MedicalProject.Models.Product.DTOs
{
    public class ProductFilterParam : BaseFilterParam
    {
        public string? Title { get; set; }
        public ProductStatus? Status { get; set; }

    }
    public class ProductFilterData : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string Slug { get; set; }
        public SeoData SeoData { get; set; }
        public ProductStatus Status { get; set; }

        public InventoryDto? InventoryDto { get; set; }

    }
   
    public class ProductFilterForIndexPageData : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string Slug { get; set; }
        public SeoData SeoData { get; set; }
        public ProductStatus Status { get; set; }

        public List<PurchaseReportDto>? PurchaseReportDto { get; set; }
        public InventoryDto? InventoryDto { get; set; }

    }

    public class ProductFilterForIndexPageResult : BaseFilter<ProductFilterForIndexPageData, ProductFilterParam>
    {
    }

    public class ProductFilterResult : BaseFilter<ProductFilterData, ProductFilterParam>
    {
    }


    public class InventoryDto : BaseDto
    {
        public Guid ProductId { get; set; }
        public string TotalPrice { get; set; }
        public int Dong { get; set; }
        //سود هر دانگ
        //public int DongPurchase { get; set; }
        public string Profit { get; set; }
        public PaymentTime ProfitableTime { get; set; }
        public string PricePerDong
        {
            get
            {
                if (decimal.TryParse(TotalPrice, out var total) && Dong > 0)
                {
                    var price = total / Dong;
                    return price.ToString("N0"); // فرمت عددی با جداکننده هزارگان
                }
                return "0";
            }
        }
    }

    public enum PaymentTime
    {
        ماهانه,
        سه_ماهه,
        شش_ماهه,
        سالانه
    }
}
