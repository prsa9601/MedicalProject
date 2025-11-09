using MedicalProject.Models;
using MedicalProject.Models.Inventory;
using MedicalProject.Models.Product;
using System.Threading;

namespace MedicalProject.Services.Product
{
    public interface IProductInventoryService
    {
        Task<ApiResult> Create(AddInventoryCommand command);
        Task<ApiResult> Edit(EditInventoryCommand command);
        Task<ApiResult> SetProfitable(SetInventoryProfitableCommand command);

    }
    public class ProductInventoryService : IProductInventoryService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Inventory";

        public ProductInventoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(AddInventoryCommand command)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/Create", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Edit(EditInventoryCommand command)
        {
            var result = await _client.PatchAsJsonAsync($"{ModuleName}/Edit", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> SetProfitable(SetInventoryProfitableCommand command)
        {
            var result = await _client.PatchAsJsonAsync($"{ModuleName}/SetProfitable", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }
    }
}
