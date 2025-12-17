using MedicalProject.Models;
using MedicalProject.Models.Auth;
using MedicalProject.Models.Contact;
using MedicalProject.Models.Order;
using MedicalProject.Models.SiteSetting;
using System.Net.Http.Json;
using System.Threading;

namespace MedicalProject.Services.Contact
{
    public interface IContactService
    {
        Task<ApiResult> Create(CreateContactCommand command);
        Task<ApiResult> Answered(ContactAnsweredCommand command);
        Task<ContactFilterResult> GetFilter(ContactFilterParam param);
      

    }

    public class ContactService : IContactService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Contact";

        public ContactService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CreateContactCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/CreateContact", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Answered(ContactAnsweredCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/AnsweredContact", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ContactFilterResult> GetFilter(ContactFilterParam param)
        {
            string url = $"{ModuleName}/GetContactByFilter?take={param.Take}&pageId={param.PageId}";
          
            if (param.Status != null)
                url += $"&status={param.Status}";
            if (param.FullName is not null)
                url += $"&FullName={param.FullName}";
            if (param.Email is not null)
                url += $"&Email={param.Email}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<ContactFilterResult>>(url);
            return result?.Data;
        }
    }
}
