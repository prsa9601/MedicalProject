using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.User;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.User;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MedicalProject.Pages.Admin.Documents
{
    public class IndexModel : BaseRazorFilter<UserFilterForDocumentsParam>
    {
        private readonly IUserService _service;

        public IndexModel(IUserService service)
        {
            _service = service;
        }

        public UserFilterForDocumentsResult? Result { get; set; }
        public async Task OnGet()
        {
            Result = await _service.GetUserByFilterForDocuments(new UserFilterForDocumentsParam
            {
                IsActive = FilterParams.IsActive,
                PageId = FilterParams.PageId,
                PhoneNumber = FilterParams.PhoneNumber,
                Take = FilterParams.Take,
                UserName = FilterParams.UserName,
                UserStatus = FilterParams.UserStatus,
            });
        }
        public async Task OnPostReject(Guid userId, string reason)
        {
            var result = await _service.ChangeUserDocumentStatus(
                new ChangeUserDocumentStatusCommand
                {
                    UserId = userId,
                    Status = Models.User.Enum.UserDocumentStatus.WrongInformation,
                }
            );
        }
        public async Task OnPostApprove(Guid userId)
        {
            var result = await _service.ChangeUserDocumentStatus(
            new ChangeUserDocumentStatusCommand
            {
                UserId = userId,
                Status = Models.User.Enum.UserDocumentStatus.IsConfirmed,
            });
        }
    }
}
