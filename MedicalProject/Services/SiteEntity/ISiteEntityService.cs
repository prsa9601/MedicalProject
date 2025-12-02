using MedicalProject.Models;
using MedicalProject.Models.SiteEntity;

namespace MedicalProject.Services.SiteEntity
{
    public interface ISiteEntityService
    {
        Task<MainPageDto?> GetMainPage();
    }
    public sealed class SiteEntityService : ISiteEntityService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "SiteEntity";

        public SiteEntityService(HttpClient client)
        {
            _client = client;
        }

        public async Task<MainPageDto?> GetMainPage()
        {
            var result = await _client.GetFromJsonAsync<ApiResult<MainPageDto?>>($"{ModuleName}/GetMainPage");
            return result?.Data;
        }
    }
}
