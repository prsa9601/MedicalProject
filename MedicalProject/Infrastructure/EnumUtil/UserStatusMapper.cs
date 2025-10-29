using MedicalProject.Models.User.Enum;

namespace MedicalProject.Infrastructure.EnumUtil
{
    public static class UserStatusMapper
    {
        public static string Map(this UserStatus status)
        {
            return status switch
            {
                UserStatus.NotConfirmed => "اطلاعات تایید نشده",
                UserStatus.WrongInformation => "مشکوک اطلاعات اشتباه",
                UserStatus.AwaitingConfirmation => "در انتظار تایید",
                UserStatus.IsConfirmed => "اطلاعات تایید شده",
                _ => "اطلاعات تایید نشده",
            };
        }
    }
}
