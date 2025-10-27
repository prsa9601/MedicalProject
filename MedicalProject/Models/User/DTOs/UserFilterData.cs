using MedicalProject.Models.User.Enum;

namespace MedicalProject.Models.User.DTOs
{
    public class UserFilterData : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalityCode { get; set; }
        public string ImageName { get; set; }
        public string NationalCardPhoto { get; set; }
        public string BirthCertificatePhoto { get; set; }
        public UserStatus Status { get; set; } 
        public bool IsActive { get; set; } //کاربر اکتیوه و میتونه کار کنه

        public UserRoleDto? UserRole { get; set; }

        public UserBankAccountDto? BankAccount { get; set; }

        public List<UserOtpDto>? UserOtps { get; set; }
        public List<UserBlockDto>? UserBlocks { get; set; }
        public List<UserAttemptDto>? UserAttempts { get; set; }
        public List<UserSessionDto>? UserSessions { get; set; }

    }
}
