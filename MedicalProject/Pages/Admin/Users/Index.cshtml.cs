using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Users
{
    public class IndexModel : BaseRazorFilter<UserFilterParam>
    {
        private readonly IUserService _service;

        public IndexModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public UserFilterResult Users { get; set; }
        public async Task OnGet()
        {
            Users = await _service.GetUserByFilter(FilterParams);
        }

        public async Task<IActionResult> OnPostDelete(Guid userId)
        {
            try
            {
                var result = await _service.Remove(userId);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "کاربر با موفقیت حذف شد";
                }
                else
                {
                    TempData["ErrorMessage"] = "خطا در حذف کاربر";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"خطا: {ex.Message}";
            }

            return RedirectToPage();
        }

        //public async Task<IActionResult> OnPostToggleUserStatusAsync(Guid userId, bool isActive)
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient("API");
        //        var response = await client.PutAsync($"api/users/{userId}/status/{isActive}", null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["Success"] = "وضعیت کاربر با موفقیت به‌روزرسانی شد";
        //        }
        //        else
        //        {
        //            TempData["Error"] = "خطا در به‌روزرسانی وضعیت کاربر";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "خطا در ارتباط با سرور";
        //    }

        //    return RedirectToPage();
        //}

        //public async Task<IActionResult> OnPostBlockUserAsync(Guid userId, string reason)
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient("API");
        //        var blockRequest = new { UserId = userId, Description = reason, BlockToDate = DateTime.Now.AddDays(7) };
        //        var response = await client.PostAsJsonAsync("api/users/block", blockRequest);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["Success"] = "کاربر با موفقیت مسدود شد";
        //        }
        //        else
        //        {
        //            TempData["Error"] = "خطا در مسدود کردن کاربر";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "خطا در ارتباط با سرور";
        //    }

        //    return RedirectToPage();
        //}

        //private async Task LoadUsers()
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient("API");
        //        var response = await client.PostAsJsonAsync("api/users/filter", FilterParams);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Users = await response.Content.ReadFromJsonAsync<UserFilterResult>();
        //        }
        //        else
        //        {
        //            Users = new UserFilterResult { Data = new List<UserFilterData>() };
        //            TempData["Error"] = "خطا در دریافت اطلاعات کاربران";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Users = new UserFilterResult { Data = new List<UserFilterData>() };
        //        TempData["Error"] = "خطا در ارتباط با سرور";
        //    }
        //}
    }
}