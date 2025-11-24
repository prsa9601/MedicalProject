using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Users
{
    public class IdentityVerificationModel : PageModel
    {
        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }

        // اطلاعات هویتی
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCardImage { get; set; }
        public string BirthCertificateImage { get; set; }
        public string SelfieWithNationalCard { get; set; }
        public bool NationalCardVerified { get; set; }
        public bool BirthCertificateVerified { get; set; }
        public bool SelfieVerified { get; set; }
        public string IdentityStatus { get; set; }

        // اطلاعات بانکی
        public string BankName { get; set; }
        public string ShebaNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string CardNumber { get; set; }
        public DateTime BankInfoRegistrationDate { get; set; }
        public string BankStatus { get; set; }

        // وضعیت کلی
        public string OverallStatus { get; set; }

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

            // اطلاعات هویتی
            NationalCode = "0012345678";
            BirthDate = new DateTime(1990, 5, 15);
            NationalCardImage = "/images/documents/national-card-sample.jpg";
            BirthCertificateImage = "/images/documents/birth-certificate-sample.jpg";
            SelfieWithNationalCard = "/images/documents/selfie-sample.jpg";
            NationalCardVerified = true;
            BirthCertificateVerified = false;
            SelfieVerified = true;
            IdentityStatus = "در انتظار تایید";

            // اطلاعات بانکی
            BankName = "ملی ایران";
            ShebaNumber = "IR120570023980010000000000";
            AccountNumber = "1234567890";
            AccountHolderName = "محمد احمدی";
            CardNumber = "6037991234567890";
            BankInfoRegistrationDate = DateTime.Now.AddMonths(-3);
            BankStatus = "در انتظار تایید";

            OverallStatus = "در انتظار تایید";

            return Page();
        }

        public async Task<IActionResult> OnPostVerifyDocumentAsync(Guid userId, string documentType, string actionType, string description)
        {
            // اینجا عملیات تایید/رد مدرک انجام می‌شود
            // برای نمونه، فقط یک پیام موفقیت برگردانده می‌شود

            TempData["SuccessMessage"] = $"مدرک {GetDocumentPersianName(documentType)} با موفقیت {GetActionPersianName(actionType)} شد";
            return RedirectToPage(new { userId });
        }

        public async Task<IActionResult> OnPostVerifyIdentityAsync(Guid userId, string description)
        {
            // اینجا عملیات تایید هویت انجام می‌شود
            TempData["SuccessMessage"] = "هویت کاربر با موفقیت تایید شد";
            return RedirectToPage(new { userId });
        }

        public async Task<IActionResult> OnPostVerifyBankInfoAsync(Guid userId, string description)
        {
            // اینجا عملیات تایید اطلاعات بانکی انجام می‌شود
            TempData["SuccessMessage"] = "اطلاعات بانکی کاربر با موفقیت تایید شد";
            return RedirectToPage(new { userId });
        }

        private string GetDocumentPersianName(string documentType)
        {
            return documentType switch
            {
                "nationalCard" => "کارت ملی",
                "birthCertificate" => "شناسنامه",
                "selfie" => "سلفی با کارت ملی",
                _ => "مدرک"
            };
        }

        private string GetActionPersianName(string actionType)
        {
            return actionType switch
            {
                "verify" => "تایید",
                "reject" => "رد",
                _ => "بررسی"
            };
        }
    }

 
}