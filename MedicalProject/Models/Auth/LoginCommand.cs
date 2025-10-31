namespace MedicalProject.Models.Auth
{
    public class LoginCommand
    {
        public required string phoneNumber { get; set; }
        public required string password { get; set; }
        public bool rememberMe { get; set; }
        public required string ipAddress { get; set; }
    }
    public class RegisterCommand
    {
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    public class VerificationOtpCodeCommand 
    {
        public string phoneNumber { get; set; }
        public string token { get; set; }
    }
    public class GenerateAndSendOtpCodeCommand
    {
        public string phoneNumber { get; set; }
    }

}
