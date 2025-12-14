using MedicalProject.Models;
using MedicalProject.Models.Auth;
using MedicalProject.Models.Contract;
using MedicalProject.Models.Order;
using MedicalProject.Models.SiteSetting;
using System.Net.Http.Json;
using System.Threading;

namespace MedicalProject.Services.Contract
{
    public interface IContractService
    {
        Task<ApiResult> Create(CreateContractCommand command);
        Task<ApiResult> Answered(ContractAnsweredCommand command);
        Task<ContractFilterResult> GetFilter(ContractFilterParam param);
      

    }

    public class ContractService : IContractService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Contract";

        public ContractService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CreateContractCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/CreateContract", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Answered(ContractAnsweredCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/AnsweredContract", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ContractFilterResult> GetFilter(ContractFilterParam param)
        {
            string url = $"{ModuleName}/GetContractByFilter?take={param.Take}&pageId={param.PageId}";
          
            if (param.Status != null)
                url += $"&status={param.Status}";
            if (param.FullName is not null)
                url += $"&FullName={param.FullName}";
            if (param.Email is not null)
                url += $"&Email={param.Email}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<ContractFilterResult>>(url);
            return result?.Data;
        }
    }
}
