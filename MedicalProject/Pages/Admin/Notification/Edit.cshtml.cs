using MedicalProject.Models.Notification;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Notification;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Notification
{
    public class EditModel : PageModel
    {
        private readonly INotificationService _service;
        private readonly IUserService _userService;

        public EditModel(INotificationService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [BindProperty(SupportsGet = true)]
        public NotificationDto? Notification { get; set; }



        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string? Link { get; set; }

        [BindProperty]
        public List<Guid> UserIds { get; set; }

        [BindProperty]
        public string NotificationType { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserFilterResult Users { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Notification = await _service.GetByIdForAdmin(id);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _service.Edit(new EditNotificationCommand
            {
                NotificationId = Notification.Id,
                Title = Title,
                Description = Notification.Description,
                Link = Link,
                SendToAll = NotificationType == "broadcast" ? true : false,
                UserId = UserIds,
            });
            if (result.IsSuccess)
            {
                TempData["Success"] = "نوتیفیکیشن با موفقیت ویرایش شد";
                return Redirect($"/Admin/Notification/Detail/{Notification.Id}");
            }
            else
            {
                return Page();
            }
        }



        public async Task<JsonResult> OnPostSearchUsersAsync([FromBody] UserSearchRequest request)
        {
            try
            {
                var users = await _userService.GetUserByFilter(new UserFilterParam
                {
                    IsActive = true,
                    Search = request.SearchTerm,
                });

                return new JsonResult(new { success = true, data = users.Data });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }
        public class UserSearchRequest
        {
            public string SearchTerm { get; set; }
        }
    }
}
