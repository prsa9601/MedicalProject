using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.BankInfo
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

        public async Task<IActionResult> OnGet()
        {
            Users = await _service.GetUserByFilter(FilterParams);
            return Page();
        }

        public async Task<IActionResult> OnPostVerifyBankInfo(Guid userId)
        {
            try
            {
                var result = await _service.ChangeConfirmationBankAccount(new Models.User.ChangeConfirmationBankAccountCommand
                {
                    UserId = userId,
                    IsConfirmed = true
                });
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "اطلاعات بانکی با موفقیت تایید شد";
                    return RedirectToPage();
                }
                else
                {
                    TempData["ErrorMessage"] = "خطا در تایید اطلاعات بانکی";
                    return RedirectToPage();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در تایید اطلاعات بانکی";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostRejectBankInfo(Guid userId, string reason)
        {
            try
            {
                var result = await _service.ChangeConfirmationBankAccount(new Models.User.ChangeConfirmationBankAccountCommand
                {
                    UserId = userId,
                    IsConfirmed = false
                });
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "اطلاعات بانکی با موفقیت رد شد";
                    return RedirectToPage();
                }
                else
                {
                    TempData["ErrorMessage"] = "خطا در رد اطلاعات بانکی";
                    return RedirectToPage();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در رد اطلاعات بانکی";
                return RedirectToPage();
            }
        }

        //public async Task<IActionResult> OnPostUpdateBankInfo(Guid userId, UpdateBankInfoModel model)
        //{
        //    try
        //    {
        //        var result = await _service.UpdateBankAccount(userId, new BankAccountUpdateDto
        //        {
        //            Shaba = model.Shaba,
        //            CardNumber = model.CardNumber,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            ExpirationDateMonth = model.ExpirationDateMonth,
        //            ExpirationDateYear = model.ExpirationDateYear
        //        });

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "اطلاعات بانکی با موفقیت بروزرسانی شد";
        //            return RedirectToPage();
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "خطا در بروزرسانی اطلاعات بانکی";
        //            return RedirectToPage();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "خطا در بروزرسانی اطلاعات بانکی";
        //        return RedirectToPage();
        //    }
        //}

        //public async Task<IActionResult> OnPostBulkVerify(string userIds)
        //{
        //    try
        //    {
        //        var userGuidList = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(userIds);
        //        var result = await _service.BulkVerifyBankAccounts(userGuidList);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "اطلاعات بانکی انتخاب شده با موفقیت تایید شدند";
        //            return RedirectToPage();
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "خطا در تایید گروهی اطلاعات بانکی";
        //            return RedirectToPage();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "خطا در تایید گروهی اطلاعات بانکی";
        //        return RedirectToPage();
        //    }
        //}

        //    public async Task<IActionResult> OnPostBulkReject(string userIds, string reason)
        //    {
        //        try
        //        {
        //            var userGuidList = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(userIds);
        //            var result = await _userService.BulkRejectBankAccounts(userGuidList, reason);

        //            if (result)
        //            {
        //                TempData["SuccessMessage"] = "اطلاعات بانکی انتخاب شده با موفقیت رد شدند";
        //                return RedirectToPage();
        //            }
        //            else
        //            {
        //                TempData["ErrorMessage"] = "خطا در رد گروهی اطلاعات بانکی";
        //                return RedirectToPage();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["ErrorMessage"] = "خطا در رد گروهی اطلاعات بانکی";
        //            return RedirectToPage();
        //        }
        //    }
    }

    public class UpdateBankInfoModel
    {
        [Required(ErrorMessage = "شماره شبا الزامی است")]
        public string Shaba { get; set; }

        [Required(ErrorMessage = "شماره کارت الزامی است")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "نام صاحب حساب الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی صاحب حساب الزامی است")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "ماه انقضا الزامی است")]
        [Range(1, 12, ErrorMessage = "ماه انقضا باید بین 1 تا 12 باشد")]
        public int ExpirationDateMonth { get; set; }

        [Required(ErrorMessage = "سال انقضا الزامی است")]
        [Range(1400, 1500, ErrorMessage = "سال انقضا باید بین 1400 تا 1500 باشد")]
        public int ExpirationDateYear { get; set; }
    }

    public class BankAccountUpdateDto
    {
        public string Shaba { get; set; }
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExpirationDateMonth { get; set; }
        public int ExpirationDateYear { get; set; }
    }
}