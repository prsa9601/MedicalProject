using MedicalProject.Models.User.Enum;

namespace MedicalProject.Models.User
{
    public class CreateUserCommand
    {
        public required string phoneNumber { get; set; }
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public required string password { get; set; }
    }
    public class EditUserCommand
    {
        public Guid userId { get; set; }
        public required string phoneNumber { get; set; }
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public string? password { get; set; }
    }
    public class CreateUserForAdminCommand
    {
        public required string phoneNumber { get; set; }
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public required string password { get; set; }
    }
    public class ChangePasswordCommand
    {
        public Guid userId { get; set; }
        public string ipAddress { get; set; }
        public required string password { get; set; }
    }

    public class CompletionOfInformationCommand
    {
        public Guid userId { get; set; }
        public string nationalityCode { get; set; }
        public IFormFile nationalCardPhoto { get; set; }
        public IFormFile birthCertificatePhoto { get; set; }
    }
    public class ConfirmedAccountUserCommand
    {
        public Guid userId { get; set; }
        public UserStatus userStatus { get; set; }
    }
    public class ChangeActivityUserAccountCommand
    {
        public Guid userId { get; set; }
        public bool Activity { get; set; }
    }
    public class SetImageUserCommand 
    {
        public Guid userId { get; set; }
        public IFormFile userAccountImage { get; set; }
    }
}
