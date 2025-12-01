using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Notification;
using MedicalProject.Services.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Notification
{
    public class IndexModel : BaseRazorFilter<NotificationFilterParam>
    {
        private readonly INotificationService _service;

        public IndexModel(INotificationService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public NotificationFilterResult Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetFilterForAdmin(FilterParams);
        }
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var result = await _service.Delete(id);
            if (result.IsSuccess)
            {
                Result = await _service.GetFilterForAdmin(FilterParams);
                TempData["Success"] = "حذف با موفقیت انجام شد.";
                return Page();
            }
            else
            {
                Result = await _service.GetFilterForAdmin(FilterParams);
                TempData["Error"] = result.MetaData.Message;
                return Page();
            }
        }
    }
}
