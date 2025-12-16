using MedicalProject.Models;
using MedicalProject.Models.Profit;

namespace MedicalProject.Services.Profit
{
    public interface IProfitService
    {
        Task<ApiResult> Create(CreateProfitCommand command);
    }
    public class ProfitService : IProfitService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Profit";

        public ProfitService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CreateProfitCommand command)
        {
            using var form = new MultipartFormDataContent();


            form.Add(new StringContent(command.OrderId.ToString()), "OrderId");

            form.Add(new StringContent(command.ProductId.ToString()), "ProductId");
            form.Add(new StringContent(command.ForWhateTime.ToString()), "ForWhateTime");
            form.Add(new StringContent(command.ForWhatePeriod.ToString()), "ForWhatePeriod");

            if (command.Status != null)
                form.Add(new StringContent(command.Status.ToString()), "Status");

            form.Add(new StringContent(command.UserId.ToString()), "UserId");
            if (command.Image != null)
            {
                form.Add(
                        new StreamContent(command.Image.OpenReadStream()),
                        "Image",
                        command.Image.FileName
                    );
            }


            var result = await _client.PostAsync($"{ModuleName}/CreateProfit", form);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }
    }
}
