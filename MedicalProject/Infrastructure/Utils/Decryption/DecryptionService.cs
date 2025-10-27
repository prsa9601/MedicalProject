using Microsoft.AspNetCore.DataProtection;

namespace MedicalProject.Infrastructure.Utils.Decryption
{

    public class DecryptionService
    {
        private readonly IDataProtector _protector;

        public DecryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("RequestIdProtector");
        }

        public string DecryptId(string protectedId)
        {
            try
            {
                return _protector.Unprotect(protectedId);
            }
            catch (Exception ex)
            {
                // مدیریت خطا در صورت شکست رمزگشایی
                return $"Decryption failed: {ex.Message}";
            }
        }
    }
}