using MedicalProject.Models;
using MedicalProject.Models.Contact;
using MedicalProject.Models.SiteSetting;

namespace MedicalProject.Services.SiteSetting
{
    public interface ISiteSettingService
    {
         Task<ApiResult> CreateOrEdit(CreateOrEditCommand command);
        Task<ApiResult<Models.SiteSetting.SiteSetting>> Get();
    }
    internal sealed class SiteSettingService : ISiteSettingService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "SiteSetting";

        public SiteSettingService(HttpClient client)
        {
            _client = client;
        }


        public async Task<ApiResult> CreateOrEdit(CreateOrEditCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/CreateOrEditSiteSetting", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult<Models.SiteSetting.SiteSetting>> Get()
        {
            var result = await _client.GetAsync($"{ModuleName}/GetSiteSetting");
            return await result.Content.ReadFromJsonAsync<ApiResult<Models.SiteSetting.SiteSetting>>();
        }
    }
}
