using MedicalProject.Models.User.Enum;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _service;

        public EditModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        public string? phoneNumber { get; set; }
        [BindProperty]
        public string? firstName { get; set; }
        [BindProperty]
        public string? lastName { get; set; }
        [BindProperty]
        public string? password { get; set; }
        [BindProperty]
        public string? confirmPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid userId { get; set; }
        [BindProperty]
        public string? nationalityCode { get; set; }
        [BindProperty]
        public IFormFile? nationalCardPhoto { get; set; }
        [BindProperty]
        public IFormFile? birthCertificatePhoto { get; set; }
        //[BindProperty(false)]
        public bool? isActive { get; set; } = true;
        [BindProperty]
        public UserDocumentStatus userStatus { get; set; } = UserDocumentStatus.NotConfirmed;
        [BindProperty]
        public IFormFile? userAccountImage { get; set; }

        public async Task<IActionResult> OnGet(Guid userId)
        {
            var user = await _service.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            // پر کردن مدل با اطلاعات کاربر
            userId = user.Id;
            firstName = user.FirstName;
            lastName = user.LastName;
            phoneNumber = user.PhoneNumber;
            isActive = user.IsActive;
            //userStatus = user.Status;
            // سایر فیلدها...

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // اعتبارسنجی رمز عبور
                if (!string.IsNullOrEmpty(password) && password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "رمز عبور و تکرار آن مطابقت ندارند");
                    return Page();
                }

                // آپلود فایل‌ها
                string nationalCardPhotoPath = null;
                string birthCertificatePhotoPath = null;
                string userAccountImagePath = null;


                var userResponse = await _service.Edit(new Models.User.EditUserCommand
                {
                    firstName = this.firstName,
                    lastName = this.lastName,
                    password = this.password,
                    phoneNumber = this.phoneNumber,
                    userId = userId,
                });

                if (birthCertificatePhoto != null
                    && nationalCardPhoto != null
                    && nationalityCode != null)
                {
                    await _service.CompletionOfInformation(new Models.User.CompletionOfInformationCommand
                    {
                        birthCertificatePhoto = birthCertificatePhoto,
                        nationalCardPhoto = nationalCardPhoto,
                        nationalityCode = nationalityCode,
                        userId = userId,
                    });
                }

                //await _service.ChangeActivityAccount(new Models.User.ChangeActivityUserAccountCommand
                //{
                //    Activity = isActive,
                //    userId = userResponse.Data
                //});

                if (password != null && confirmPassword != null)
                {
                    if (password == confirmPassword)
                    {
                        await _service.ChangePassword(new Models.User.ChangePasswordCommand
                        {
                            userId = userId,
                            password = password
                        });
                    }
                }

                await _service.ConfirmedAccount(new Models.User.ConfirmedAccountUserCommand
                {
                    userId = userId,
                    userStatus = userStatus,
                });

                if (userAccountImage != null)
                {

                    await _service.SetImage(new Models.User.SetImageUserCommand
                    {
                        userId = userId,
                        userAccountImage = userAccountImage,
                    });
                }

                if (userResponse.IsSuccess)
                {
                    TempData["SuccessMessage"] = "کاربر با موفقیت بروزرسانی شد";
                    return RedirectToPage("/Admin/Users/Index");
                }
                else
                {
                    ModelState.AddModelError("", userResponse.MetaData.ToString());
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در بروزرسانی کاربر: " + ex.Message);
                return Page();
            }
        }
    }
}
