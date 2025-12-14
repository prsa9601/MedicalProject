using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactFormModel ContactForm { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "لطفاً تمام فیلدهای ضروری را پر کنید.";
                IsSuccess = false;
                return Page();
            }

            try
            {
                // اینجا کد ارسال ایمیل یا ذخیره در دیتابیس را قرار دهید
                // مثلا:
                // _emailService.SendContactEmail(ContactForm);
                // _context.ContactMessages.Add(contactMessage);
                // await _context.SaveChangesAsync();

                Message = "پیام شما با موفقیت ارسال شد. به زودی با شما تماس خواهیم گرفت.";
                IsSuccess = true;

                // ریست کردن فرم
                ContactForm = new ContactFormModel();

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                Message = "متاسفانه در ارسال پیام خطایی رخ داد. لطفاً دوباره تلاش کنید.";
                IsSuccess = false;
            }

            return Page();
        }
    }

    public class ContactFormModel
    {
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [Display(Name = "نام و نام خانوادگی")]
        [StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "شماره تماس الزامی است")]
        [Phone(ErrorMessage = "شماره تماس معتبر نیست")]
        [Display(Name = "شماره تماس")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "موضوع الزامی است")]
        [Display(Name = "موضوع")]
        [StringLength(200, ErrorMessage = "موضوع نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "پیام الزامی است")]
        [Display(Name = "پیام")]
        [StringLength(5000, ErrorMessage = "پیام نمی‌تواند بیشتر از ۵۰۰۰ کاراکتر باشد")]
        public string Message { get; set; }
    }
}