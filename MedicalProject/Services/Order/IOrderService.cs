using MedicalProject.Models;
using MedicalProject.Models.Order;
using MedicalProject.Models.Product;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;
using System;
using System.Threading;

namespace MedicalProject.Services.Order
{
    public interface IOrderService
    {
        Task<ApiResult> Create(CancellationToken cancellationToken);
        Task<ApiResult> IsFinally(OrderIsFinallyViewModel command, CancellationToken cancellationToken);
        Task<ApiResult> SetOrderItem(SetOrderItemCommandViewModel command, CancellationToken cancellationToken);


        Task<OrderDto?> GetById(Guid orderId, CancellationToken cancellationToken);
        Task<OrderDto?> GetCurrentUser();
        Task<OrderFilterResult> GetFilter(OrderFilterParam param, CancellationToken cancellationToken);
        Task<OrderFilterResult> GetForReport(OrderFilterParam param, CancellationToken cancellationToken);
    }
    internal class OrderService : IOrderService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Order";

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CancellationToken cancellationToken)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/Create", cancellationToken);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> IsFinally(OrderIsFinallyViewModel command, CancellationToken cancellationToken)
        {
            var result = await _client.PostAsJsonAsync($"{ModuleName}/IsFinally", command, cancellationToken);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<OrderDto?> GetById(Guid orderId, CancellationToken cancellationToken)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<OrderDto?>>($"{ModuleName}/GetOrderById?orderId={orderId}", cancellationToken);
            return result?.Data;
        }

        public async Task<OrderFilterResult> GetFilter(OrderFilterParam param, CancellationToken cancellationToken)
        {
            string url = $"{ModuleName}/GetOrdersByFilter?take={param.Take}&pageId={param.PageId}";
            if (param.EndDate != DateTime.MaxValue && param.EndDate != DateTime.MinValue)
                url += $"&EndDate={param.EndDate}";
            if (param.StartDate != DateTime.MaxValue && param.StartDate != DateTime.MinValue)
                url += $"&StartDate={param.StartDate}";
            if (param.OrderFilter is not null && param.OrderFilter != OrderFilter.None)
                url += $"&OrderFilter={param.OrderFilter}";
            if (param.ProductId is not null)
                url += $"&ProductId={param.ProductId}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<OrderFilterResult>>(url, cancellationToken);
            return result?.Data;
        }

        public async Task<OrderFilterResult> GetForReport(OrderFilterParam param, CancellationToken cancellationToken)
        {
            string url = $"{ModuleName}/GetOrdersForReport?take={param.Take}&pageId={param.PageId}";
            if (param.EndDate != DateTime.MaxValue && param.EndDate != DateTime.MinValue)
                url += $"&EndDate={param.EndDate}";
            if (param.StartDate != DateTime.MaxValue && param.StartDate != DateTime.MinValue)
                url += $"&StartDate={param.StartDate}";
            if (param.OrderFilter is not null && param.OrderFilter != OrderFilter.None)
                url += $"&OrderFilter={param.OrderFilter}";
            if (param.ProductId is not null)
                url += $"&ProductId={param.ProductId}";
            if (param.PhoneNumber is not null)
                url += $"&PhoneNumber={param.PhoneNumber}";

            var result = await _client.GetFromJsonAsync<ApiResult<OrderFilterResult>>(url, cancellationToken);
            return result?.Data;
        }

        public async Task<ApiResult> SetOrderItem(SetOrderItemCommandViewModel command, CancellationToken cancellationToken)
        {
            var result = await _client.PatchAsJsonAsync($"{ModuleName}/SetOrderItem", command, cancellationToken);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<OrderDto?> GetCurrentUser()
        {
            var result = await _client.GetFromJsonAsync<ApiResult<OrderDto>>($"{ModuleName}/GetCurrentUser");
            return result?.Data;
        }
    }
}
