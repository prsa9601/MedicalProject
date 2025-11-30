using MedicalProject.Models;
using MedicalProject.Models.Auth;
using MedicalProject.Models.Notification;
using MedicalProject.Models.Order;
using System;
using System.Threading;

namespace MedicalProject.Services.Notification
{
    public interface INotificationService
    {
        Task<ApiResult> Create(CreateNotificationCommand command);
        Task<ApiResult> CreateList(CreateListCommand command);
        Task<ApiResult> Edit(EditNotificationCommand command);
        Task<ApiResult> Delete(Guid id);
        Task<NotificationFilterResult> GetFilterForAdmin(NotificationFilterParam param);
        Task<NotificationFilterResultForUser> GetFilterForCurrentUser(NotificationFilterParam param);
        Task<NotificationDtoForUser?> GetById(Guid id, Guid userId);
        Task<NotificationDto?> GetByIdForAdmin(Guid id);
    }
    internal class NotificationService : INotificationService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Notification";

        public NotificationService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CreateNotificationCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/CreateNotification", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> CreateList(CreateListCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/CreateListNotification", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Edit(EditNotificationCommand command)
        {
            var result = await _client.PatchAsJsonAsync($"{ModuleName}/EditNotification", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Delete(Guid id)
        {
            var result = await _client.DeleteAsync($"{ModuleName}/DeleteNotification?id={id}");
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<NotificationFilterResult> GetFilterForAdmin(NotificationFilterParam param)
        {
            string url = $"{ModuleName}/GetFilterForAdmin?take={param.Take}&pageId={param.PageId}";

            if (param.Title is not null)
                url += $"&Title={param.Title}";

            if (param.Description is not null)
                url += $"&Description={param.Description}";

            var result = await _client.GetFromJsonAsync<ApiResult<NotificationFilterResult>>(url);
            return result?.Data;
        }

        public async Task<NotificationFilterResultForUser> GetFilterForCurrentUser(NotificationFilterParam param)
        {
            string url = $"{ModuleName}/GetFilterForCurrentUser?take={param.Take}&pageId={param.PageId}";
    
            if (param.Title is not null)
                url += $"&Title={param.Title}";

            if (param.Description is not null)
                url += $"&Description={param.Description}";

            var result = await _client.GetFromJsonAsync<ApiResult<NotificationFilterResultForUser>>(url);
            return result?.Data;
        }

        public async Task<NotificationDtoForUser?> GetById(Guid id, Guid userId)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<NotificationDtoForUser>>($"{ModuleName}/GetById?id={id}&userId={userId}");
            return result?.Data;
        }

        public async Task<NotificationDto?> GetByIdForAdmin(Guid id)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<NotificationDto?>>($"{ModuleName}/GetByIdForAdmin/{id}");
            return result?.Data;
        }
    }
}
 