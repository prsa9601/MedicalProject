using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Order;
using MedicalProject.Services.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Order
{
    public class IndexModel : BaseRazorFilter<OrderFilterParam>
    {
        private readonly IOrderService _service;

        public IndexModel(IOrderService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public OrderFilterResult Order { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Order = await _service.GetFilter(FilterParams, CancellationToken.None);
            return Page();
        }
    }
}
