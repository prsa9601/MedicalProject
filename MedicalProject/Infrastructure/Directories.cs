namespace MedicalProject.Infrastructure
{
    public class Directories
    {
        public static string GetUserBirthCertificatePhotoPath(string imageName)
            => $"{SiteSettings.ServerPath}{UserBirthCertificatePhotoPath.Replace("wwwroot", "")}/{imageName}";

        public static string GetUserImageAccountPath(string imageName)
            => $"{SiteSettings.ServerPath}{UserImageAccountPath.Replace("wwwroot", "")}/{imageName}";
        public static string GetUserNationalCardPhotoPath(string imageName)

            => $"{SiteSettings.ServerPath}{UserNationalCardPhotoPath.Replace("wwwroot", "")}/{imageName}";

        public const string UserImageAccountPath = "/images/user/account";
        public const string UserNationalCardPhotoPath = "/images/user/nationalityCode";
        public const string UserBirthCertificatePhotoPath = "/images/user/birthCertificate";

    }
}