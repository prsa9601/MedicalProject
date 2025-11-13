using MedicalProject.Models.User.Enum;

namespace MedicalProject.Infrastructure.EnumUtil
{
    public static class UserStatusMapper
    {
        public static string Map(this UserDocumentStatus status)
        {
            return status switch
            {
                UserDocumentStatus.NotConfirmed => "اطلاعات تایید نشده",
                UserDocumentStatus.WrongInformation => "مشکوک اطلاعات اشتباه",
                UserDocumentStatus.AwaitingConfirmation => "در انتظار تایید",
                UserDocumentStatus.IsConfirmed => "اطلاعات تایید شده",
                _ => "اطلاعات تایید نشده",
            };
        }
    }
}
