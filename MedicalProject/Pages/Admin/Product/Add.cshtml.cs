using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Product
{
    [BindProperties]
    //[Area("Admin")]
    public class AddModel : PageModel
    {
        public required string slug { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public bool IndexPage { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }
        public void OnGet()
        {
        }
    }
}
