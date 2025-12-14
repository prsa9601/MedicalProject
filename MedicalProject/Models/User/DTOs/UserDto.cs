using MedicalProject.Infrastructure;
using MedicalProject.Models.User.Enum;

namespace MedicalProject.Models.User.DTOs
{
    public class UserDto : BaseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageName { get; set; }
        public bool IsActive { get; set; }
        public UserRoleDto? UserRole { get; set; }
        public UserDocumentDto? UserDocument { get; set; }


        public UserBankAccountDto? BankAccount { get; set; }
    }

    public class UserSessionDto : BaseDto
    {
        public Guid UserId { get; set; }
        public string JwtRefreshToken { get; set; }

        //public string JwtAuthToken { get; set; }

        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; } // اگه کاربر لاگ اوت کنه فالس میشه
        public string IpAddress { get; set; }

    }
    public class UserRoleDto : BaseDto
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }
    public class UserOtpDto : BaseDto
    {
        public Guid UserId { get; set; }
        public string OtpCode { get; set; }
        public DateTime ExpireDate { get; set; }
    }
    public class UserBlockDto : BaseDto
    {
        public Guid UserId { get; set; }
        public DateTime BlockToDate { get; set; }
        public string Description { get; set; } //چرا بلاک شده
        public bool IsActive { get; set; }
    }
    public class UserAttemptDto : BaseDto
    {
        public Guid UserId { get; set; }
        public DateTime AttemptDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string FailureReason { get; set; }
        public DateTime ExpireDate { get; set; } // ✅ فیلد جدید
        public AttemptType AttemptType { get; set; }
    }
    public class UserBankAccountDto
    {
        public string Shaba { get; set; }
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid UserId { get; set; }
    }
}
