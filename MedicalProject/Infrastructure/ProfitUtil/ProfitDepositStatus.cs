using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.PurchaseReport;

namespace MedicalProject.Infrastructure.ProfitUtil
{
    public class ProfitDepositStatus
    {
        public static bool CheckStatus(UserPurchaseReportDto userPurchase)
        {
            if (userPurchase?.PurchaseReport == null || userPurchase.ProductPurchase == null)
                return false;

            foreach (var purchaseItem in userPurchase.PurchaseReport)
            {
                var products = userPurchase.ProductPurchase.Where(i => i.Id.Equals(purchaseItem.ProductId));

                foreach (var product in products)
                {
                    if (product.InventoryDto?.ProfitableTime == null)
                        continue;

                    var creationDate = purchaseItem.CreationDate;
                    int daysToAdd = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);

                    if (daysToAdd == 0) continue;

                    int paymentNumber = CalculatePaymentNumber(creationDate, daysToAdd);

                    if (paymentNumber == 0) continue;

                    var profitRecords = userPurchase.ProfitPurchases?
                        .Where(p => p.ProductId.Equals(product.Id) && p.UserId.Equals(userPurchase.UserId))
                        .OrderBy(p => p.ForWhatPeriod)
                        .ToList() ?? new List<ProfitPurchaseReportDto>();

                    // بررسی اینکه آیا برای تمام دوره‌ها سود واریز شده
                    for (int period = 1; period <= paymentNumber; period++)
                    {
                        if (!profitRecords.Any(p => p.ForWhatPeriod == period))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static int GetDaysByPaymentTime(PaymentTime paymentTime)
        {
            return paymentTime switch
            {
                PaymentTime.ماهانه => 30,
                PaymentTime.سه_ماهه => 90,
                PaymentTime.شش_ماهه => 180,
                PaymentTime.سالانه => 365,
                _ => 0
            };
        }

        private static int CalculatePaymentNumber(DateTime creationDate, int daysPerPeriod)
        {
            if (DateTime.Now <= creationDate)
                return 0;

            int paymentNumber = 0;
            DateTime currentDate = creationDate;

            while (currentDate <= DateTime.Now)
            {
                paymentNumber++;
                currentDate = currentDate.AddDays(daysPerPeriod);
            }

            // یک دوره کم می‌کنیم چون آخرین دوره ممکن است کامل نباشد
            return paymentNumber - 1;
        }

        public static Dictionary<Guid, List<ProfitPurchaseReportDto>> GetUserProfitPaidStatus(UserPurchaseReportDto userPurchase)
        {
            var result = new Dictionary<Guid, List<ProfitPurchaseReportDto>>();

            if (userPurchase?.PurchaseReport == null || userPurchase.ProductPurchase == null)
                return result;

            foreach (var purchaseItem in userPurchase.PurchaseReport)
            {
                var products = userPurchase.ProductPurchase.Where(p => p.Id.Equals(purchaseItem.ProductId));

                foreach (var product in products)
                {
                    if (product.InventoryDto?.ProfitableTime == null)
                        continue;

                    var productId = product.Id;
                    var paidProfits = GetPaidProfitsForProduct(userPurchase, product, purchaseItem);

                    if (!result.ContainsKey(productId))
                    {
                        result[productId] = new List<ProfitPurchaseReportDto>();
                    }

                    result[productId].AddRange(paidProfits);
                }
            }

            return result;
        }

        private static List<ProfitPurchaseReportDto> GetPaidProfitsForProduct(
            UserPurchaseReportDto userPurchase,
            ProductPurchaseReportDto product,
            PurchaseReportDto purchaseItem)
        {
            var paidProfits = new List<ProfitPurchaseReportDto>();
            var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);

            if (daysPerPeriod == 0) return paidProfits;

            int expectedPaymentCount = CalculateExpectedPaymentCount(purchaseItem.CreationDate, daysPerPeriod);

            if (expectedPaymentCount == 0) return paidProfits;

            // پیدا کردن سودهای واریز شده برای این محصول
            var existingProfits = userPurchase.ProfitPurchases?
                .Where(p => p.ProductId.Equals(product.Id) &&
                           p.UserId.Equals(userPurchase.UserId))
                .OrderBy(p => p.ForWhatPeriod)
                .ToList() ?? new List<ProfitPurchaseReportDto>();

            // برای هر دوره مورد انتظار، بررسی کن آیا سود واریز شده
            for (int period = 1; period <= expectedPaymentCount; period++)
            {
                var profitForPeriod = existingProfits
                    .FirstOrDefault(p => p.ForWhatPeriod == period);

                if (profitForPeriod != null)
                {
                    paidProfits.Add(profitForPeriod);
                }
            }

            return paidProfits;
        }



        private static int CalculateExpectedPaymentCount(DateTime creationDate, int daysPerPeriod)
        {
            if (DateTime.Now <= creationDate)
                return 0;

            double totalDaysPassed = (DateTime.Now - creationDate).TotalDays;
            int completedPeriods = (int)(totalDaysPassed / daysPerPeriod);

            return completedPeriods;
        }


        // متد برای دریافت وضعیت کامل پرداخت‌ها
        public static ProfitStatusResult GetUserProfitStatus(UserPurchaseReportDto userPurchase)
        {
            var paidProfits = new List<ProfitPurchaseReportDto>();
            var unpaidPeriods = new List<UnpaidPeriodInfo>();

            if (userPurchase?.PurchaseReport == null || userPurchase.ProductPurchase == null)
                return new ProfitStatusResult { PaidProfits = paidProfits, UnpaidPeriods = unpaidPeriods };

            foreach (var purchaseItem in userPurchase.PurchaseReport)
            {
                var products = userPurchase.ProductPurchase.Where(p => p.Id.Equals(purchaseItem.ProductId));

                foreach (var product in products)
                {
                    if (product.InventoryDto?.ProfitableTime == null)
                        continue;

                    var productStatus = GetProfitStatusForProduct(userPurchase, product, purchaseItem);
                    paidProfits.AddRange(productStatus.PaidProfits);
                    unpaidPeriods.AddRange(productStatus.UnpaidPeriods);
                }
            }

            return new ProfitStatusResult
            {
                PaidProfits = paidProfits.OrderByDescending(p => p.CreationDate).ToList(),
                UnpaidPeriods = unpaidPeriods.OrderBy(u => u.DueDate).ToList()
            };
        }
        public static List<ProfitStatusResult> GetUsersProfitStatus(List<UserPurchaseReportDto> userPurchases)
        {
            var results = new List<ProfitStatusResult>();

            if (userPurchases == null || !userPurchases.Any())
                return results;

            foreach (var userPurchase in userPurchases)
            {
                var paidProfits = new List<ProfitPurchaseReportDto>();
                var unpaidPeriods = new List<UnpaidPeriodInfo>();

                if (userPurchase?.PurchaseReport == null || userPurchase.ProductPurchase == null)
                {
                    results.Add(new ProfitStatusResult { PaidProfits = paidProfits, UnpaidPeriods = unpaidPeriods });
                    continue;
                }

                // استفاده از Dictionary برای جلوگیری از محاسبات تکراری
                var processedProducts = new HashSet<Guid>();

                foreach (var purchaseItem in userPurchase.PurchaseReport)
                {
                    // اگر این محصول قبلاً پردازش شده، ردش کن
                    if (processedProducts.Contains(purchaseItem.ProductId))
                        continue;

                    var products = userPurchase.ProductPurchase.Where(p => p.Id.Equals(purchaseItem.ProductId)).ToList();

                    foreach (var product in products)
                    {
                        if (product.InventoryDto?.ProfitableTime == null)
                            continue;

                        var productStatus = GetProfitStatusForProduct(userPurchase, product, purchaseItem);

                        // اضافه کردن به لیست‌ها با بررسی تکراری نبودن
                        AddUniqueProfits(paidProfits, productStatus.PaidProfits);
                        AddUniqueUnpaidPeriods(unpaidPeriods, productStatus.UnpaidPeriods);

                        // علامت گذاری محصول به عنوان پردازش شده
                        processedProducts.Add(product.Id);
                    }
                }

                results.Add(new ProfitStatusResult
                {
                    PaidProfits = paidProfits.OrderByDescending(p => p.CreationDate).ToList(),
                    UnpaidPeriods = unpaidPeriods.OrderBy(u => u.DueDate).ToList()
                });
            }

            return results;
        }

        // متدهای کمکی برای اضافه کردن مقادیر منحصر به فرد
        private static void AddUniqueProfits(List<ProfitPurchaseReportDto> target, IEnumerable<ProfitPurchaseReportDto> source)
        {
            if (source == null) return;

            foreach (var profit in source)
            {
                if (profit == null) continue;

                // بررسی وجود داشتن بر اساس ID یا ترکیبی از فیلدها
                if (!target.Any(p =>
                    p.Id == profit.Id ||
                    (p.ProductId == profit.ProductId && p.CreationDate == profit.CreationDate && p.AmountPaid == profit.AmountPaid)))
                {
                    target.Add(profit);
                }
            }
        }

        private static void AddUniqueUnpaidPeriods(List<UnpaidPeriodInfo> target, IEnumerable<UnpaidPeriodInfo> source)
        {
            if (source == null) return;

            foreach (var period in source)
            {
                if (period == null) continue;

                // بررسی وجود داشتن بر اساس ترکیبی از فیلدها
                if (!target.Any(u =>
                    u.ProductId == period.ProductId &&
                    u.PeriodNumber == period.PeriodNumber &&
                    u.ExpectedAmount == period.ExpectedAmount &&
                    u.DueDate == period.DueDate))
                {
                    target.Add(period);
                }
            }
        }
        private static ProductProfitStatus GetProfitStatusForProduct(
            UserPurchaseReportDto userPurchase,
            ProductPurchaseReportDto product,
            PurchaseReportDto purchaseItem)
        {
            var paidProfits = new List<ProfitPurchaseReportDto>();
            var unpaidPeriods = new List<UnpaidPeriodInfo>();

            var daysPerPeriod = GetDaysByPaymentTime(product.InventoryDto.ProfitableTime);
            if (daysPerPeriod == 0)
                return new ProductProfitStatus { PaidProfits = paidProfits, UnpaidPeriods = unpaidPeriods };

            int expectedPaymentCount = CalculateExpectedPaymentCount(purchaseItem.CreationDate, daysPerPeriod);
            if (expectedPaymentCount == 0)
                return new ProductProfitStatus { PaidProfits = paidProfits, UnpaidPeriods = unpaidPeriods };

            var existingProfits = userPurchase.ProfitPurchases?
                .Where(p => p.ProductId.Equals(product.Id) &&
                           p.UserId.Equals(userPurchase.UserId))
                .ToList() ?? new List<ProfitPurchaseReportDto>();

            for (int period = 1; period <= expectedPaymentCount; period++)
            {
                var profitForPeriod = existingProfits
                    .FirstOrDefault(p => p.ForWhatPeriod == period);

                if (profitForPeriod != null)
                {
                    paidProfits.Add(profitForPeriod);
                }
                else
                {
                    unpaidPeriods.Add(new UnpaidPeriodInfo
                    {
                        PeriodNumber = period,
                        DueDate = CalculateDueDate(purchaseItem.CreationDate, period, daysPerPeriod),
                        ProductName = product.Title,
                        ExpectedAmount = CalculateExpectedAmount(product, period),
                        ProductId = product.Id
                    });
                }
            }

            return new ProductProfitStatus
            {
                PaidProfits = paidProfits,
                UnpaidPeriods = unpaidPeriods
            };
        }


        private static DateTime CalculateDueDate(DateTime creationDate, int period, int daysPerPeriod)
        {
            return creationDate.AddDays(period * daysPerPeriod);
        }

        private static decimal CalculateExpectedAmount(ProductPurchaseReportDto product, decimal period)
        {
            // اینجا منطق محاسبه مبلغ سود بر اساس محصول و دوره را قرار دهید
            // به عنوان مثال:
            return decimal.Parse(product.InventoryDto.Profit)/* * period*/; // 10% سود
        }
    }

    // کلاس‌های مدل
    public class ProfitStatusResult
    {
        public List<ProfitPurchaseReportDto> PaidProfits { get; set; }
            = new List<ProfitPurchaseReportDto>();

        public List<UnpaidPeriodInfo> UnpaidPeriods { get; set; }
            = new List<UnpaidPeriodInfo>();
    }

    public class ProductProfitStatus
    {
        public List<ProfitPurchaseReportDto> PaidProfits { get; set; }
            = new List<ProfitPurchaseReportDto>();

        public List<UnpaidPeriodInfo> UnpaidPeriods { get; set; }
            = new List<UnpaidPeriodInfo>();
    }

    public class UnpaidPeriodInfo
    {
        public int PeriodNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string ProductName { get; set; }
        public decimal ExpectedAmount { get; set; }
        public Guid ProductId { get; set; }
    }
}