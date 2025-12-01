using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.Notification;
using MedicalProject.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace MedicalProject.Pages.Front.Notifications
{
    public class IndexModel : BaseRazorFilter<NotificationFilterParam>
    {
        private readonly INotificationService _service;

        public IndexModel(INotificationService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public NotificationFilterResultForUser Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetFilterForCurrentUser(FilterParams);
        }
    }
}
