using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.BankInfo
{
    public class BankInfoModel : PageModel
    {
        private readonly IUserService _service;

        public BankInfoModel(IUserService service)
        {
            _service = service;
        }

        public UserBankAccountDto? Result { get; set; }

        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string BankName { get; set; }
        public string CardNumber { get; set; }
        public string ShebaNumber { get; set; }
        public string CardExpiry { get; set; }
        public string NationalCode { get; set; }
        public DateTime RegistrationDate { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            var result = await _service.GetUserById(userId);

            if (result == null)
            {
                TempData["Error"] = "حساب بانکی کاربر یافت نشده است.";
                return Redirect("/Admin/Finance");
            }

            Result = result.BankAccount;

            // اینجا اطلاعات کاربر از دیتابیس لود می‌شود
            UserId = userId;

            // نمونه اطلاعات تستی - در عمل از دیتابیس لود می‌شود
            UserProfile = new UserProfile
            {
                FirstName = "محمد",
                LastName = "احمدی",
                PhoneNumber = "09121234567",
                CreationDate = DateTime.Now.AddMonths(-6)
            };

            BankName = "ملی ایران";
            CardNumber = "6037991234567890";
            ShebaNumber = "IR120570023980010000000000";
            CardExpiry = "08/26";
            NationalCode = "0012345678";
            RegistrationDate = DateTime.Now.AddMonths(-3);

            return Page();
        }
    }

    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
    }
}