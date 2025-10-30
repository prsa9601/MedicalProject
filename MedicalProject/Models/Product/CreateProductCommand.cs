namespace MedicalProject.Models.Product
{
    public class CreateProductCommand
    {
        public required string slug { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public ProductStatus status { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public bool? IndexPage { get; set; }
        public IFormFile Image { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }
    }
    public class EditProductCommand
    {
        public required string slug { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public ProductStatus status { get; set; }
        public IFormFile Image { get; set; }
        public bool? IndexPage { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }
        public Guid productId { get; set; }
    }
    public class RemoveProductCommand
    {
        public Guid productId { get; set; }
    }
    public enum ProductStatus
    {
        IsActive, // فعال است
        NotActive, //فعال نیست
        IsDone, //تکمیل شده
    }
}
