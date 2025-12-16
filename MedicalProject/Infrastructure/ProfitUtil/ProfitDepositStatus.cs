using MedicalProject.Infrastructure.Utils;
using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;

namespace MedicalProject.Infrastructure.ProfitUtil
{
    public class ProfitDepositStatus
    {
        public static bool CheckStatus(UserPurchaseReportDto userPurchase)
        {
            if (userPurchase?.OrderDtos == null || !userPurchase.OrderDtos.Any())
                return true;

            // فقط سفارشات پرداخت شده را بررسی کن
            var paidOrders = userPurchase.OrderDtos.Where(o => o.status == OrderStatus.paid).ToList();

            foreach (var order in paidOrders)
            {
                if (order?.OrderItems == null) continue;

                var product = userPurchase.ProductPurchase?.FirstOrDefault(p => p.Id == order.OrderItems.ProductId);
                if (product == null) continue;

                if (product?.InventoryDto?.ProfitableTime == null) continue;

                var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);
                if (daysPerPeriod == 0) continue;

                // محاسبه تعداد دوره‌های گذشته از تاریخ خرید
                var timeSincePurchase = DateTime.Now - order.CreationDate;
                var totalDaysPassed = timeSincePurchase.TotalDays;
                var expectedPeriods = (int)(totalDaysPassed / daysPerPeriod);

                if (expectedPeriods == 0) continue;

                // سودهای پرداخت شده برای این محصول در این سفارش
                var paidProfits = userPurchase.ProfitPurchases?
                    .Where(p => p.ProductId == product.Id &&
                               p.OrderId == order.Id &&
                               p.Status == ProfitStatus.Success)
                    .ToList() ?? new List<ProfitPurchaseReportDto>();

                // اگر تعداد سودهای پرداخت شده کمتر از دوره‌های مورد انتظار باشد، کاربر بدهکار است
                if (paidProfits.Count < expectedPeriods)
                {
                    return false; // بدهکار
                }
            }

            return true; // بی‌حساب
        }

        public static ProfitStatusResult GetUserProfitStatus(UserPurchaseReportDto userPurchase)
        {
            var result = new ProfitStatusResult();

            if (userPurchase?.OrderDtos == null || !userPurchase.OrderDtos.Any())
                return result;

            // فقط سفارشات پرداخت شده
            var paidOrders = userPurchase.OrderDtos.Where(o => o.status == OrderStatus.paid).ToList();

            foreach (var order in paidOrders)
            {
                if (order?.OrderItems == null) continue;

                var product = userPurchase.ProductPurchase?.FirstOrDefault(p => p.Id == order.OrderItems.ProductId);
                if (product?.InventoryDto?.ProfitableTime == null) continue;

                var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);
                if (daysPerPeriod == 0) continue;

                // محاسبه دوره‌های مورد انتظار
                var timeSincePurchase = DateTime.Now - order.CreationDate;
                var totalDaysPassed = timeSincePurchase.TotalDays;
                var expectedPeriods = (int)(totalDaysPassed / daysPerPeriod);

                if (expectedPeriods == 0) continue;

                // سودهای پرداخت شده برای این محصول
                var paidProfits = userPurchase.ProfitPurchases?
                    .Where(p => p.ProductId == product.Id &&
                               p.OrderId == order.Id &&
                               p.Status == ProfitStatus.Success)
                    .OrderBy(p => p.ForWhatPeriod)
                    .ToList() ?? new List<ProfitPurchaseReportDto>();

                // اضافه کردن سودهای پرداخت شده
                result.PaidProfits.AddRange(paidProfits);

                // محاسبه دوره‌های پرداخت نشده
                for (int period = 1; period <= expectedPeriods; period++)
                {
                    // اگر این دوره پرداخت نشده است
                    if (!paidProfits.Any(p => p.ForWhatPeriod == period))
                    {
                        var dueDate = order.CreationDate.AddDays(period * daysPerPeriod);
                        var expectedAmount = (decimal.Parse(product.InventoryDto.Profit)*order.OrderItems.DongAmount)/6 ;//CalculateExpectedAmount(product, order.OrderItems);

                        result.UnpaidPeriods.Add(new UnpaidPeriodInfo
                        {
                            PeriodNumber = period,
                            DueDate = dueDate,
                            ProductName = product.Title,
                            ExpectedAmount = expectedAmount,
                            ProductId = product.Id,
                            OrderId = order.Id,
                            OrderItemId = order.OrderItems.Id
                        });
                    }
                }
            }

            // حذف موارد تکراری
            result.PaidProfits = result.PaidProfits
                .GroupBy(p => new { p.ProductId, p.OrderId, p.ForWhatPeriod })
                .Select(g => g.First())
                .OrderByDescending(p => p.CreationDate)
                .ToList();

            result.UnpaidPeriods = result.UnpaidPeriods
                .GroupBy(u => new { u.ProductId, u.OrderId, u.PeriodNumber })
                .Select(g => g.First())
                .OrderBy(u => u.DueDate)
                .ToList();

            return result;
        }

        public static List<ProfitStatusResult> GetUsersProfitStatus(List<UserPurchaseReportDto> userPurchases)
        {
            var results = new List<ProfitStatusResult>();

            if (userPurchases == null || !userPurchases.Any())
                return results;

            foreach (var userPurchase in userPurchases)
            {
                results.Add(GetUserProfitStatus(userPurchase));
            }

            return results;
        }

        public static decimal GetTotalUnpaidAmount(List<UserPurchaseReportDto> userPurchases)
        {
            if (userPurchases == null || !userPurchases.Any())
                return 0;

            try
            {
                var allUnpaidPeriods = GetUsersProfitStatus(userPurchases)
                    .SelectMany(r => r.UnpaidPeriods)
                    .Where(p => p != null)
                    .ToList();

                return allUnpaidPeriods.Sum(p => p.ExpectedAmount);
            }
            catch
            {
                return 0;
            }
        }

        public static string GetTotalUnpaidAmountFormatted(List<UserPurchaseReportDto> userPurchases)
        {
            return GetTotalUnpaidAmount(userPurchases).ToString("N0");
        }

        // متد جدید برای دریافت اطلاعات کامل کاربر
        public static UserProfitSummary GetUserProfitSummary(UserPurchaseReportDto userPurchase)
        {
            var summary = new UserProfitSummary
            {
                UserId = userPurchase?.UserId ?? Guid.Empty,
                UserName = $"{userPurchase?.FirstName} {userPurchase?.Lastame}",
                PhoneNumber = userPurchase?.PhoneNumber
            };

            if (userPurchase?.OrderDtos != null)
            {
                // کل سرمایه‌گذاری
                summary.TotalInvestment = userPurchase.OrderDtos
                    .Where(o => o.status == OrderStatus.paid)
                    .Sum(o => o.OrderItems?.TotalPrice ?? 0);

                // کل سود پرداخت شده
                summary.TotalPaidProfit = userPurchase.ProfitPurchases?
                    .Where(p => p.Status == ProfitStatus.Success)
                    .Sum(p => p.AmountPaid) ?? 0;

                // وضعیت فعلی
                var profitStatus = GetUserProfitStatus(userPurchase);
                summary.TotalUnpaidProfit = profitStatus.UnpaidPeriods.Sum(p => p.ExpectedAmount);
                summary.IsDebtor = !CheckStatus(userPurchase);
                summary.UnpaidPeriodsCount = profitStatus.UnpaidPeriods.Count;
                summary.PaidPeriodsCount = profitStatus.PaidProfits.Count;

                // جزئیات هر سفارش
                foreach (var order in userPurchase.OrderDtos.Where(o => o.status == OrderStatus.paid))
                {
                    if (order.OrderItems == null) continue;

                    var orderSummary = new OrderProfitSummary
                    {
                        OrderId = order.Id,
                        OrderDate = order.CreationDate,
                        TotalAmount = order.OrderItems.TotalPrice
                    };

                    var product = userPurchase.ProductPurchase?.FirstOrDefault(p => p.Id == order.OrderItems.ProductId);
                    if (product != null)
                    {
                        var productSummary = new ProductProfitSummary
                        {
                            ProductName = product.Title,
                            ProductId = product.Id,
                            DongAmount = order.OrderItems.DongAmount,
                            TotalInvestment = order.OrderItems.TotalPrice
                        };

                        // سودهای پرداخت شده برای این محصول
                        var paidProfits = userPurchase.ProfitPurchases?
                            .Where(p => p.ProductId == product.Id &&
                                       p.OrderId == order.Id &&
                                       p.Status == ProfitStatus.Success)
                            .ToList() ?? new List<ProfitPurchaseReportDto>();

                        productSummary.PaidProfit = paidProfits.Sum(p => p.AmountPaid);
                        productSummary.PaidPeriods = paidProfits.Count;

                        // محاسبه سود مورد انتظار
                        if (product.InventoryDto?.ProfitableTime != null)
                        {
                            var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);
                            var timeSincePurchase = DateTime.Now - order.CreationDate;
                            var totalDaysPassed = timeSincePurchase.TotalDays;
                            var expectedPeriods = (int)(totalDaysPassed / daysPerPeriod);

                            var expectedAmountPerPeriod = CalculateExpectedAmount(product, order.OrderItems);
                            productSummary.ExpectedProfit = expectedAmountPerPeriod * expectedPeriods;
                            productSummary.UnpaidProfit = Math.Max(0, productSummary.ExpectedProfit - productSummary.PaidProfit);
                            productSummary.ExpectedPeriods = expectedPeriods;
                        }

                        orderSummary.Products.Add(productSummary);
                    }

                    orderSummary.TotalPaidProfit = orderSummary.Products.Sum(p => p.PaidProfit);
                    orderSummary.TotalUnpaidProfit = orderSummary.Products.Sum(p => p.UnpaidProfit);
                    orderSummary.TotalExpectedProfit = orderSummary.Products.Sum(p => p.ExpectedProfit);

                    summary.Orders.Add(orderSummary);
                }

                summary.TotalUnpaidProfit = summary.Orders.Sum(o => o.TotalUnpaidProfit);
                summary.TotalPaidProfit = summary.Orders.Sum(o => o.TotalPaidProfit);
                summary.TotalExpectedProfit = summary.Orders.Sum(o => o.TotalExpectedProfit);
            }

            return summary;
        }

        // متد کمکی برای تست
        public static string GetDebugInfo(UserPurchaseReportDto userPurchase)
        {
            var summary = GetUserProfitSummary(userPurchase);
            var profitStatus = GetUserProfitStatus(userPurchase);
            var checkStatus = CheckStatus(userPurchase);

            var debugInfo = new List<string>
            {
                $"=== اطلاعات دیباگ برای کاربر: {summary.UserName} ===",
                $"شماره تماس: {summary.PhoneNumber}",
                $"وضعیت: {(checkStatus ? "بی‌حساب" : "بدهکار")}",
                $"سرمایه‌گذاری کل: {summary.TotalInvestment.ToString("N0")} ریال",
                $"سود پرداخت شده: {summary.TotalPaidProfit.ToString("N0")} ریال",
                $"سود پرداخت نشده: {summary.TotalUnpaidProfit.ToString("N0")} ریال",
                $"دوره‌های پرداخت شده: {summary.PaidPeriodsCount}",
                $"دوره‌های پرداخت نشده: {summary.UnpaidPeriodsCount}",
                ""
            };

            if (userPurchase?.OrderDtos != null)
            {
                var paidOrders = userPurchase.OrderDtos.Where(o => o.status == OrderStatus.paid).ToList();
                debugInfo.Add($"تعداد سفارشات پرداخت شده: {paidOrders.Count}");

                foreach (var order in paidOrders)
                {
                    debugInfo.Add($"");
                    debugInfo.Add($"سفارش: {order.Id} - تاریخ: {order.CreationDate.ToPersianDate()} - مبلغ: {(order.OrderItems?.TotalPrice ?? 0).ToString("N0")} ریال");

                    if (order.OrderItems != null)
                    {
                        var product = userPurchase.ProductPurchase?.FirstOrDefault(p => p.Id == order.OrderItems.ProductId);
                        if (product != null)
                        {
                            var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto?.ProfitableTime ?? PaymentTime.ماهانه);
                            var timeSincePurchase = DateTime.Now - order.CreationDate;
                            var expectedPeriods = (int)(timeSincePurchase.TotalDays / daysPerPeriod);

                            var paidProfits = userPurchase.ProfitPurchases?
                                .Where(p => p.ProductId == product.Id && p.OrderId == order.Id && p.Status == ProfitStatus.Success)
                                .ToList() ?? new List<ProfitPurchaseReportDto>();

                            debugInfo.Add($"  محصول: {product.Title}");
                            debugInfo.Add($"    دنگ: {order.OrderItems.DongAmount} - قیمت کل: {order.OrderItems.TotalPrice.ToString("N0")} ریال");
                            debugInfo.Add($"    دوره‌های مورد انتظار: {expectedPeriods}");
                            debugInfo.Add($"    دوره‌های پرداخت شده: {paidProfits.Count}");
                            debugInfo.Add($"    وضعیت: {(paidProfits.Count >= expectedPeriods ? "تسویه شده" : "بدهکار")}");
                        }
                    }
                }
            }

            return string.Join(Environment.NewLine, debugInfo);
        }

        // محاسبات کمکی
        private static int GetDaysByPaymentTime(PaymentTime paymentTime)
        {
            return paymentTime switch
            {
                PaymentTime.ماهانه => 30,
                PaymentTime.سه_ماهه => 90,
                PaymentTime.شش_ماهه => 180,
                PaymentTime.سالانه => 365,
                _ => 30
            };
        }

        private static decimal CalculateExpectedAmount(ProductPurchaseReportDto product, OrderItemDto orderItem)
        {
            try
            {
                // محاسبه سود بر اساس درصد سود محصول
                if (product.InventoryDto != null && !string.IsNullOrEmpty(product.InventoryDto.Profit))
                {
                    if (decimal.TryParse(product.InventoryDto.Profit, out decimal profitPercentage) && profitPercentage > 0)
                    {
                        return (orderItem?.TotalPrice ?? 0) * (profitPercentage / 100m);
                    }
                }

                // اگر درصد سود مشخص نیست، 10% از سرمایه‌گذاری
                return (orderItem?.TotalPrice ?? 0) * 0.1m;
            }
            catch
            {
                return (orderItem?.TotalPrice ?? 0) * 0.1m; // 10% پیش‌فرض
            }
        }
    }

    // کلاس‌های مدل
    public class ProfitStatusResult
    {
        public List<ProfitPurchaseReportDto> PaidProfits { get; set; } = new();
        public List<UnpaidPeriodInfo> UnpaidPeriods { get; set; } = new();
    }

    public class UnpaidPeriodInfo
    {
        public int PeriodNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string ProductName { get; set; }
        public decimal ExpectedAmount { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
    }

    // کلاس‌های جدید برای خلاصه اطلاعات
    public class UserProfitSummary
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalPaidProfit { get; set; }
        public decimal TotalUnpaidProfit { get; set; }
        public decimal TotalExpectedProfit { get; set; }
        public bool IsDebtor { get; set; }
        public int PaidPeriodsCount { get; set; }
        public int UnpaidPeriodsCount { get; set; }
        public List<OrderProfitSummary> Orders { get; set; } = new();
    }

    public class OrderProfitSummary
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaidProfit { get; set; }
        public decimal TotalUnpaidProfit { get; set; }
        public decimal TotalExpectedProfit { get; set; }
        public List<ProductProfitSummary> Products { get; set; } = new();
    }

    public class ProductProfitSummary
    {
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public decimal DongAmount { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal PaidProfit { get; set; }
        public decimal UnpaidProfit { get; set; }
        public decimal ExpectedProfit { get; set; }
        public int PaidPeriods { get; set; }
        public int ExpectedPeriods { get; set; }
    }
}