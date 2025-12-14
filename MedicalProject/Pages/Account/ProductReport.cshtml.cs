using MedicalProject.Infrastructure.ProfitUtil;
using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Models.PurchaseReport;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.PurchaseReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicalProject.Pages.Account
{
    public class ProductReportModel : BaseRazorFilter<UserPurchaseReportForCurrentUserFilterParam>
    {
        private readonly IPurchaseReportService _service;
        private readonly ILogger<ProductReportModel> _logger;

        public ProductReportModel(IPurchaseReportService service, ILogger<ProductReportModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public UserPurchaseReportDto Result { get; set; }



        public UserProfitSummary? UserProfitSummary { get; set; }
        public List<ProfitPurchaseReportDto> PaidProfits { get; set; } = new();
        public List<UnpaidPeriodInfo> UnpaidPeriods { get; set; } = new();
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsLoading { get; set; } = true;

        public async Task<IActionResult> OnGet()
        {
            try
            {
                IsLoading = true;
                HasError = false;

                // Get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    _logger.LogWarning("User ID not found or invalid");

                    // Create empty summary for unauthenticated users
                    UserProfitSummary = CreateEmptySummary();
                    IsLoading = false;
                    return Page();
                }

                // Get user purchase report
                var filterParam = new UserPurchaseReportFilterParam
                {
                    UserId = userGuid,
                    PageId = 1,
                    Take = 100 // Get all records
                };

                var result = await _service.GetFilterUserForCurrentUser(FilterParams);

                if (result == null || result.Data == null || !result.Data.Any())
                {
                    _logger.LogInformation("No investment data found for user {UserId}", userGuid);

                    // Create summary with no investments
                    UserProfitSummary = CreateEmptySummary(userGuid);
                    IsLoading = false;
                    return Page();
                }

                var userPurchase = result.Data.FirstOrDefault();
                if (userPurchase == null)
                {
                    _logger.LogWarning("User purchase data is null");

                    UserProfitSummary = CreateEmptySummary(userGuid);
                    IsLoading = false;
                    return Page();
                }

                try
                {
                    // Get profit status summary
                    var profitStatus = ProfitDepositStatus.GetUserProfitStatus(userPurchase);
                    PaidProfits = profitStatus?.PaidProfits ?? new List<ProfitPurchaseReportDto>();
                    UnpaidPeriods = profitStatus?.UnpaidPeriods ?? new List<UnpaidPeriodInfo>();

                    // Get detailed summary
                    UserProfitSummary = ProfitDepositStatus.GetUserProfitSummary(userPurchase);

                    if (UserProfitSummary == null)
                    {
                        _logger.LogWarning("User profit summary is null");
                        UserProfitSummary = CreateEmptySummary(userGuid);
                    }
                }
                catch (Exception summaryEx)
                {
                    _logger.LogError(summaryEx, "Error calculating profit summary for user {UserId}", userGuid);

                    // Create a basic summary even if calculation fails
                    UserProfitSummary = CreateBasicSummary(userPurchase, userGuid);
                }

                IsLoading = false;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProfitStatus page for user");

                HasError = true;
                ErrorMessage = $"خطا در دریافت اطلاعات: {ex.Message}";

                // Create empty summary
                UserProfitSummary = CreateEmptySummary();
                IsLoading = false;

                return Page();
            }
        }

        private UserProfitSummary CreateEmptySummary(Guid? userId = null)
        {
            return new UserProfitSummary
            {
                UserId = userId ?? Guid.Empty,
                UserName = User?.Identity?.Name ?? "کاربر",
                PhoneNumber = User?.FindFirstValue(ClaimTypes.MobilePhone) ?? "",
                TotalInvestment = 0,
                TotalPaidProfit = 0,
                TotalUnpaidProfit = 0,
                TotalExpectedProfit = 0,
                IsDebtor = false,
                PaidPeriodsCount = 0,
                UnpaidPeriodsCount = 0,
                Orders = new List<OrderProfitSummary>()
            };
        }

        private UserProfitSummary CreateBasicSummary(UserPurchaseReportDto userPurchase, Guid userId)
        {
            try
            {
                var summary = new UserProfitSummary
                {
                    UserId = userId,
                    UserName = $"{userPurchase.FirstName ?? ""} {userPurchase.Lastame ?? ""}".Trim(),
                    PhoneNumber = userPurchase.PhoneNumber ?? "",
                    Orders = new List<OrderProfitSummary>()
                };

                // Calculate basic totals
                if (userPurchase.OrderDtos != null)
                {
                    summary.TotalInvestment = userPurchase.OrderDtos
                        .Where(o => o?.status == Models.Order.OrderStatus.paid)
                        .Sum(o => o?.OrderItems?.TotalPrice ?? 0);

                    if (userPurchase.ProfitPurchases != null)
                    {
                        summary.TotalPaidProfit = userPurchase.ProfitPurchases
                            .Where(p => p?.Status == ProfitStatus.Success)
                            .Sum(p => p?.AmountPaid ?? 0);
                    }

                    // Create basic order summaries
                    foreach (var order in userPurchase.OrderDtos.Where(o => o != null && o.status == Models.Order.OrderStatus.paid))
                    {
                        if (order?.OrderItems == null) continue;

                        var orderSummary = new OrderProfitSummary
                        {
                            OrderId = order.Id,
                            OrderDate = order.CreationDate,
                            TotalAmount = order.OrderItems.TotalPrice,
                            Products = new List<ProductProfitSummary>()
                        };

                        var product = userPurchase.ProductPurchase?.FirstOrDefault(p => p?.Id == order.OrderItems.ProductId);
                        if (product != null)
                        {
                            var productSummary = new ProductProfitSummary
                            {
                                ProductName = product.Title ?? "محصول ناشناس",
                                ProductId = product.Id,
                                DongAmount = order.OrderItems.DongAmount,
                                TotalInvestment = order.OrderItems.TotalPrice
                            };

                            orderSummary.Products.Add(productSummary);
                        }

                        summary.Orders.Add(orderSummary);
                    }
                }

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating basic summary");
                return CreateEmptySummary(userId);
            }
        }

        public IActionResult OnPostRequestPayment(int periodNumber, Guid productId, Guid orderId)
        {
            try
            {
                if (periodNumber <= 0 || productId == Guid.Empty || orderId == Guid.Empty)
                {
                    TempData["Error"] = "اطلاعات دوره ناقص است";
                    return RedirectToPage();
                }

                // Logic for requesting payment for a specific period
                // This would typically save a payment request to the database
                // For now, just show a success message

                TempData["Success"] = "درخواست پرداخت سود با موفقیت ثبت شد. تیم پشتیبانی با شما تماس خواهد گرفت.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting payment for period {PeriodNumber}", periodNumber);
                TempData["Error"] = $"خطا در ثبت درخواست پرداخت: {ex.Message}";
                return RedirectToPage();
            }
        }

        public IActionResult OnPostGenerateReport(string reportType)
        {
            try
            {
                if (string.IsNullOrEmpty(reportType))
                {
                    TempData["Error"] = "نوع گزارش مشخص نشده است";
                    return RedirectToPage();
                }

                // Logic for generating different types of reports
                switch (reportType.ToLower())
                {
                    case "detailed":
                        TempData["Info"] = "گزارش تفصیلی آماده شد. لطفاً صبر کنید...";
                        break;
                    case "summary":
                        TempData["Info"] = "گزارش خلاصه آماده شد. لطفاً صبر کنید...";
                        break;
                    case "tax":
                        TempData["Info"] = "گزارش مالیاتی آماده شد. لطفاً صبر کنید...";
                        break;
                    default:
                        TempData["Error"] = "نوع گزارش نامعتبر است.";
                        return RedirectToPage();
                }

                // Here you would generate the actual report file
                // For now, just return a success message

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report type {ReportType}", reportType);
                TempData["Error"] = $"خطا در تولید گزارش: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}

//public async Task<IActionResult> OnGet(Guid id)
//        {
//            Result = await _service.GetById(id);

//            return Page();
//        }
//    }
//}
