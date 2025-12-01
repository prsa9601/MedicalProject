using MedicalProject.Models;
using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using Microsoft.AspNetCore.Authorization;

namespace MedicalProject.Services.PurchaseReport
{
    public interface IPurchaseReportService
    {
        Task<PurchaseReportFilterResult> GetFilterForAdmin(PurchaseReportFilterParam param, CancellationToken cancellationToken);
        Task<PurchaseReportUserInvestmentFilterResult> GetFilterUserForAdmin(UserPurchaseReportFilterParam param, CancellationToken cancellationToken);
        Task<PurchaseReportUserInvestmentFilterResult> GetFilterUserForCurrentUser(UserPurchaseReportForCurrentUserFilterParam param);
        Task<UserPurchaseReportDto> GetById(Guid userId);
        Task<UserPurchaseReportDto> GetForCurrentUser();
    }
    internal class PurchaseReportService : IPurchaseReportService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "PurchaseReport";

        public PurchaseReportService(HttpClient client)
        {
            _client = client;
        }

        public async Task<PurchaseReportFilterResult> GetFilterForAdmin(PurchaseReportFilterParam param, CancellationToken cancellationToken)
        {
            string url = $"{ModuleName}/GetPurchaseReportFilterForAdmin?take={param.Take}&pageId={param.PageId}";
            if (param.EndDate != DateTime.MaxValue && param.EndDate != DateTime.MinValue)
                url += $"&EndDate={param.EndDate}";
            if (param.StartDate != DateTime.MaxValue && param.StartDate != DateTime.MinValue)
                url += $"&StartDate={param.StartDate}";
            if (param.PurchaseReportFilter is not null && param.PurchaseReportFilter != PurchaseReportFilter.None)
                url += $"&PurchaseReportFilter={param.PurchaseReportFilter}";
            if (param.ProductId is not null)
                url += $"&ProductId={param.ProductId}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<PurchaseReportFilterResult>>(url, CancellationToken.None);
            return result?.Data;
        }

        public async Task<PurchaseReportUserInvestmentFilterResult> GetFilterUserForAdmin(UserPurchaseReportFilterParam param, CancellationToken cancellationToken)
        {
            string url = $"{ModuleName}/GetPurchaseUserReportFilterForAdmin?take={param.Take}&pageId={param.PageId}";
            if (param.EndDate != DateTime.MaxValue && param.EndDate != DateTime.MinValue)
                url += $"&EndDate={param.EndDate}";
            if (param.StartDate != DateTime.MaxValue && param.StartDate != DateTime.MinValue)
                url += $"&StartDate={param.StartDate}";
            if (param.PurchaseReportFilter is not null && param.PurchaseReportFilter != PurchaseReportFilter.None)
                url += $"&PurchaseReportFilter={param.PurchaseReportFilter}";
            if (param.ProductId is not null)
                url += $"&ProductId={param.ProductId}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";
            if (param.UserId != null && param.UserId != default)
                url += $"&userId={param.UserId}";

            var result = await _client.GetFromJsonAsync<ApiResult<PurchaseReportUserInvestmentFilterResult>>(url, CancellationToken.None);
            return result?.Data;
        }

        public async Task<PurchaseReportUserInvestmentFilterResult> GetFilterUserForCurrentUser(UserPurchaseReportForCurrentUserFilterParam param)
        {
            string url = $"{ModuleName}/GetPurchaseUserReportFilterForCurrentUser?take={param.Take}&pageId={param.PageId}";
            if (param.EndDate != DateTime.MaxValue && param.EndDate != DateTime.MinValue)
                url += $"&EndDate={param.EndDate}";
            if (param.StartDate != DateTime.MaxValue && param.StartDate != DateTime.MinValue)
                url += $"&StartDate={param.StartDate}";
            if (param.PurchaseReportFilter is not null && param.PurchaseReportFilter != PurchaseReportFilter.None)
                url += $"&PurchaseReportFilter={param.PurchaseReportFilter}";
            if (param.ProductId is not null)
                url += $"&ProductId={param.ProductId}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<PurchaseReportUserInvestmentFilterResult>>(url);
            return result?.Data;
        }

        public async Task<UserPurchaseReportDto> GetById(Guid userId)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<UserPurchaseReportDto?>>($"{ModuleName}/GetById?UserId={userId}");
            return result?.Data;
        }

        [Authorize]
        public async Task<UserPurchaseReportDto> GetForCurrentUser()
        {
            var result = await _client.GetFromJsonAsync<ApiResult<UserPurchaseReportDto?>>($"{ModuleName}/GetForCurrentUser");
            return result?.Data;
        }
    }
}
