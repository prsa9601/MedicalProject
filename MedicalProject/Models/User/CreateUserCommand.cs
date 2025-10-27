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
    public class CompletionOfInformationCommand
    {
        public required Guid userId { get; set; }
        public required string nationalityCode { get; set; }
        public required IFormFile nationalCardPhoto { get; set; }
        public required IFormFile birthCertificatePhoto { get; set; }
    }
    public class ConfirmedAccountUserCommand
    {
        public Guid userId { get; set; }
        public UserStatus userStatus { get; set; }
    }
    public class SetImageUserCommand 
    {
        public Guid userId { get; set; }
        public IFormFile userAccountImage { get; set; }
    }
}
