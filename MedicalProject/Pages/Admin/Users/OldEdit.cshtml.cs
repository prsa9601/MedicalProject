using MedicalProject.Models.User.Enum;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Users
{
    [BindProperties]
    public class OldEditModel : PageModel
    {
        private readonly IUserService _service;

        public OldEditModel(IUserService service)
        {
            _service = service;
        }

        public string? phoneNumber { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? password { get; set; }
        public string? confirmPassword { get; set; }

        public Guid userId { get; set; }
        public string? nationalityCode { get; set; }
        public IFormFile? nationalCardPhoto { get; set; }
        public IFormFile? birthCertificatePhoto { get; set; }
        //[BindProperty(false)]
        public bool? isActive { get; set; } = true;
        public UserDocumentStatus userStatus { get; set; } = UserDocumentStatus.NotConfirmed;

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
            //nationalityCode = user.NationalityCode;
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
                    await _service.CompletionOfInformation(new Models.User.CompletionOfInformationCommandViewModel
                    {
                        birthCertificatePhoto = birthCertificatePhoto,
                        nationalCardPhoto = nationalCardPhoto,
                        nationalityCode = nationalityCode,
                    });
                }

                //await _service.ChangeActivityAccount(new Models.User.ChangeActivityUserAccountCommand
                //{
                //    Activity = isActive,
                //    userId = userResponse.Data
                //});

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

        //public async Task<IActionResult> OnPostDeleteAsync()
        //{
        //    try
        //    {
        //        var result = await _service.DeleteUserAsync(UserId);
        //        if (result.Success)
        //        {
        //            TempData["SuccessMessage"] = "کاربر با موفقیت حذف شد";
        //            return RedirectToPage("/Admin/Users/Index");
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = result.Message;
        //            return Page();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "خطا در حذف کاربر: " + ex.Message;
        //        return Page();
        //    }
        //}

        //public async Task<IActionResult> OnPostResetPasswordAsync()
        //{
        //    try
        //    {
        //        var result = await _service.ResetPasswordAsync(UserId);
        //        if (result.Success)
        //        {
        //            TempData["SuccessMessage"] = "رمز عبور با موفقیت بازنشانی شد";
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = result.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "خطا در بازنشانی رمز عبور: " + ex.Message;
        //    }

        //    return Page();
        //}

        private async Task<string> UploadFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folder}/{uniqueFileName}";
        }
    }
}