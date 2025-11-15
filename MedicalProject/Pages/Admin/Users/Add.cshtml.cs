using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User.Enum;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Users
{
    [BindProperties]
    public class AddModel : BaseRazorPage
    {
        private readonly IUserService _service;

        public AddModel(IUserService service)
        {
            _service = service;
        }

        public string phoneNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }

        public Guid userId { get; set; }
        public string nationalityCode { get; set; }
        public IFormFile nationalCardPhoto { get; set; }
        public IFormFile birthCertificatePhoto { get; set; }

        public bool isActive { get; set; } = true;
        public UserDocumentStatus userStatus { get; set; } = UserDocumentStatus.NotConfirmed;

        public IFormFile userAccountImage { get; set; }

        public async Task OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (password != confirmPassword)
                ModelState.AddModelError(confirmPassword,
                    "رمز عبور و تکرار رمز عبور با هم مطابقت ندارند.");
            //if (firstName != null
            //  && lastName != null
            //  && password != null
            //  && phoneNumber != null)
            //{

            var userResponse = await _service.CreateForAdmin(new Models.User.CreateUserForAdminCommand
            {
                firstName = this.firstName,
                lastName = this.lastName,
                password = this.password,
                phoneNumber = this.phoneNumber,
            });
            //}

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
                userId = userResponse.Data,
                userStatus = userStatus,
            });

            if (userAccountImage != null)
            {

                await _service.SetImage(new Models.User.SetImageUserCommand
                {
                    userId = userResponse.Data,
                    userAccountImage = userAccountImage,
                });
            }

            return RedirectAndShowAlert(new Models.ApiResult
            {
                IsReload = false,
                MetaData = userResponse.MetaData,
                IsSuccess = userResponse.IsSuccess
            }, RedirectToPage("Index"));
        }
    }
}
