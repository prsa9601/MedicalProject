using MedicalProject.Models.Notification;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Notification;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Notification
{
    public class AddModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly INotificationService _service;

        public AddModel(IUserService userService, INotificationService service)
        {
            _userService = userService;
            _service = service;
        }


        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Link { get; set; }


        public async Task<IActionResult> OnGet(string phoneNumber)
        {
            return Page();
        }


        public async Task<JsonResult> OnPostCreateNotificationAsync([FromBody] CreateNotificationRequest request)
        {

            var result = await _service.CreateList(new CreateListCommand
            {
                Link = request.Link,
                Description = request.Description,
                Title = request.Title,
                UserIds = request.SelectedUsers,
                SendToAll = request.NotificationType == "broadcast" ? true : false,
            });
            if (result.IsSuccess)
            {
                return new JsonResult(new
                {
                    success = true,
                    message = $"نوتیفیکیشن با موفقیت ارسال شد. ",
                });
            }

            return new JsonResult(new { success = false, error = "برای ارسال خاص، حداقل یک کاربر باید انتخاب شود" });
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
        public class CreateNotificationRequest
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Link { get; set; } = string.Empty;
            public string NotificationType { get; set; } = string.Empty;
            public List<Guid> SelectedUsers { get; set; } = new List<Guid>();
        }
    }
}