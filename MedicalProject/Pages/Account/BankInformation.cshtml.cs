using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Account
{
    public class BankInformationModel : PageModel
    {
        private readonly IUserService _service;

        public BankInformationModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        public string CardNumber { get; set; }

        [BindProperty]
        public string ShebaNumber { get; set; }

        [BindProperty]
        public string AccountHolderName { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserDto User { get; set; }

        public async Task<IActionResult> OnGet()
        {
            User = await _service.GetCurrentUser();
            if (User == null)
            {
                TempData["Error"] = "درخواست غیر مجاز";
                return Redirect("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _service.AddBankAccount(new Models.User.AddBankAccountCommandViewModel
            {
                CardNumber = CardNumber,
                ShabaNumber = ShebaNumber,
                FullName = AccountHolderName,
            });

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.MetaData.Message;
                return Page();
            }
            TempData["Success"] = result.MetaData.Message;
            return Redirect("/Account/Profile");
        }
    }
}
