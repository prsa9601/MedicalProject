using MedicalProject.Models.Notification;
using MedicalProject.Services.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Notification
{
    public class DetailModel : PageModel
    {
        private readonly INotificationService _service;

        public DetailModel(INotificationService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public NotificationDto? Notification { get; set; }
        public async Task OnGet(Guid id)
        {
            Notification = await _service.GetByIdForAdmin(id);
        }
    }
}
