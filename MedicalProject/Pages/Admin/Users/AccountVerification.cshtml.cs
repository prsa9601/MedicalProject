using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Users
{
    public class AccountVerificationModel : PageModel
    {
        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string AccountStatus { get; set; }
        public int VerificationScore { get; set; }
        public List<VerificationCheck> VerificationChecks { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int InvestmentCount { get; set; }
        public decimal TotalInvestment { get; set; }
        public int CreditScore { get; set; }
        public List<ActivityLog> ActivityLog { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            UserId = userId;

            // نمونه اطلاعات تستی
            UserProfile = new UserProfile
            {
                FirstName = "محمد",
                LastName = "احمدی",
                PhoneNumber = "09121234567",
                CreationDate = DateTime.Now.AddMonths(-6)
            };

            AccountStatus = "در انتظار تایید";
            VerificationScore = 75;
            LastActivityDate = DateTime.Now.AddDays(-2);
            InvestmentCount = 8;
            TotalInvestment = 125000000;
            CreditScore = 82;

            VerificationChecks = new List<VerificationCheck>
            {
                new() { Title = "تایید هویت", Description = "مدارک هویتی کاربر بررسی شده", Status = true },
                new() { Title = "تایید شماره تلفن", Description = "شماره تلفن کاربر تایید شده", Status = true },
                new() { Title = "تایید حساب بانکی", Description = "حساب بانکی کاربر فعال است", Status = true },
                new() { Title = "تایید آدرس", Description = "آدرس کاربر نیاز به بررسی دارد", Status = false },
                new() { Title = "سابقه فعالیت", Description = "کاربر فعالیت منظم دارد", Status = true }
            };

            ActivityLog = new List<ActivityLog>
            {
                new() {
                    Title = "ثبت نام کاربر",
                    Description = "کاربر در پلتفرم ثبت نام کرد",
                    Date = DateTime.Now.AddMonths(-6),
                    Type = "success",
                    Status = "تکمیل شده",
                    ActionBy = "سیستم"
                },
                new() {
                    Title = "تایید شماره تلفن",
                    Description = "شماره تلفن کاربر تایید شد",
                    Date = DateTime.Now.AddMonths(-5),
                    Type = "success",
                    Status = "تایید شده",
                    ActionBy = "سیستم"
                },
                new() {
                    Title = "اولین سرمایه‌گذاری",
                    Description = "کاربر اولین سرمایه‌گذاری خود را انجام داد",
                    Date = DateTime.Now.AddMonths(-4),
                    Type = "success",
                    Status = "تکمیل شده",
                    ActionBy = "کاربر"
                },
                new() {
                    Title = "درخواست تایید حساب",
                    Description = "کاربر درخواست تایید حساب کامل را ارسال کرد",
                    Date = DateTime.Now.AddDays(-7),
                    Type = "warning",
                    Status = "در انتظار بررسی",
                    ActionBy = "کاربر"
                }
            };

            return Page();
        }

        public async Task<IActionResult> OnPostVerifyAsync(Guid userId, string actionType, string description, int? suspensionDuration)
        {
            // اینجا عملیات تایید/رد/تعلیق حساب انجام می‌شود
            // برای نمونه، فقط یک پیام موفقیت برگردانده می‌شود

            TempData["SuccessMessage"] = $"حساب کاربر با موفقیت {GetActionPersianName(actionType)} شد";
            return RedirectToPage(new { userId });
        }

        private string GetActionPersianName(string actionType)
        {
            return actionType switch
            {
                "verify" => "تایید",
                "reject" => "رد",
                "suspend" => "تعلیق",
                _ => "بررسی"
            };
        }
    }

    public class VerificationCheck
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }

    public class ActivityLog
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // success, warning, danger
        public string Status { get; set; }
        public string ActionBy { get; set; }
    }

    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
    }
}