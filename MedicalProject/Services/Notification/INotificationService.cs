using MedicalProject.Models;

namespace MedicalProject.Services.Notification
{
    public interface INotificationService
    {
        Task<ApiResult> Create();
        Task<ApiResult> CreateList();
        Task<ApiResult> Edit();
        Task<ApiResult> Delete();
        Task<ApiResult> GetFilterForAdmin();
        Task<ApiResult> GetFilterForCurrentUser();
        Task<ApiResult> GetById(Guid id);
        Task<ApiResult> GetByIdForAdmin(Guid id);
    }
    internal class NotificationService 
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Notification";

        public NotificationService(HttpClient client)
        {
            _client = client;
        }
    }
}
