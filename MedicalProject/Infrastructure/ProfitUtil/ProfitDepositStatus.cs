using MedicalProject.Models.PurchaseReport;

namespace MedicalProject.Infrastructure.ProfitUtil
{
    public class ProfitDepositStatus
    {
        public static bool CheckStatus(UserPurchaseReportDto userPurchase)
        {

            List<ProfitPurchaseReportDto> profit = new();
            foreach (var item2 in userPurchase.PurchaseReport)
            {
                var product = userPurchase.ProductPurchase.Where(i => i.Id.Equals(item2.ProductId));
                foreach (var item in product)
                {
                    if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.ماهانه)
                    {
                        var date = item2.CreationDate;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(30);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            if (!profit.Any(i => i.ForWhatPeriod.Equals(i)))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.سه_ماهه)
                    {
                        var date = item.CreationDate;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(90);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            if (!profit.Any(i => i.ForWhatPeriod.Equals(i)))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.شش_ماهه)
                    {
                        var date = item.CreationDate;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(180);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            if (!profit.Any(i => i.ForWhatPeriod.Equals(i)))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.سالانه)
                    {
                        var date = item.CreationDate;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(3650);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            if (!profit.Any(i => i.ForWhatPeriod.Equals(i)))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }


            return true;
        }
        public static Dictionary<Guid, List<ProfitPurchaseReportDto>> GetUserProfitPaidStatus(UserPurchaseReportDto userPurchase)
        {
            List<ProfitPurchaseReportDto> profit = new();
            foreach (var item2 in userPurchase.PurchaseReport)
            {
                var product = userPurchase.ProductPurchase.Where(i => i.Id.Equals(item2.ProductId));
                foreach (var item in product)
                {
                    if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.ماهانه)
                    {
                        var date = item2.CreationDate;
                        int paymentNumber = 0;
                        int notPaid = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(30);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item2.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            var profitMatch = profit.Where(i => i.ForWhatPeriod.Equals(i) &&
                            i.UserId.Equals(userPurchase.UserId) && i.ProductId.Equals(item2.Id));

                            if (profitMatch == null)
                            {
                                notPaid++;
                            }
                            profit.AddRange(profitMatch);
                        }

                    }

                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.سه_ماهه)
                    {
                        var date = item2.CreationDate;
                        int notPaid = 0;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(90);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item2.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            var profitMatch = profit.Where(i => i.ForWhatPeriod.Equals(i) &&
                        i.UserId.Equals(userPurchase.UserId) && i.ProductId.Equals(item2.Id));

                            if (profitMatch == null)
                            {
                                notPaid++;
                            }
                            profit.AddRange(profitMatch);
                        }
                    }
                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.شش_ماهه)
                    {
                        var date = item2.CreationDate;
                        int notPaid = 0;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(180);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item2.Id) && item1.UserId.Equals(userPurchase.UserId))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            var profitMatch = profit.Where(i => i.ForWhatPeriod.Equals(i) &&
                         i.UserId.Equals(userPurchase.UserId) && i.ProductId.Equals(item2.Id));

                            if (profitMatch == null)
                            {
                                notPaid++;
                            }
                            profit.AddRange(profitMatch);
                        }
                    }
                    else if (item.InventoryDto.ProfitableTime == Models.Product.DTOs.PaymentTime.سالانه)
                    {
                        var date = item2.CreationDate;
                        int notPaid = 0;
                        int paymentNumber = 0;
                        while (date <= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
                        {
                            date = date.AddDays(3650);
                            paymentNumber++;
                        }
                        foreach (var item1 in userPurchase.ProfitPurchases)
                        {
                            if (item1.ProductId.Equals(item2.Id))
                            {
                                profit.Add(item1);
                            }
                        }
                        profit.OrderBy(i => i.ForWhatPeriod);

                        for (int i = 1; i <= paymentNumber; i++)
                        {
                            var profitMatch = profit.Where(i => i.ForWhatPeriod.Equals(i) &&
                        i.UserId.Equals(userPurchase.UserId) && i.ProductId.Equals(item2.Id));

                            if (profitMatch == null)
                            {
                                notPaid++;
                            }
                            profit.AddRange(profitMatch);
                        }
                    }
                }


            }
            return MapToDictionary(profit);

        }
        private static Dictionary<Guid, List<ProfitPurchaseReportDto>> MapToDictionary(List<ProfitPurchaseReportDto> model)
        {
            Dictionary<Guid, List<ProfitPurchaseReportDto>> result = new();

            foreach (var item in model)
            {
                //result.ContainsKey(item.ProductId)
                if (result.TryGetValue(item.ProductId, out var dictionaryParameter))
                {
                    dictionaryParameter.Add(item);
                }
                else
                {
                    result.TryAdd(item.ProductId, new List<ProfitPurchaseReportDto>
                    {
                        item
                    });
                }
            }

            return result;
        }
        //private static List<ProfitPurchaseReportDto> GetProfit(UserPurchaseReportDto userPurchase)
        //{
        //    List<ProfitPurchaseReportDto> profit = new();

        //    var date = userPurchase.CreationDate;
        //    int paymentNumber = 0;
        //    int notPaid = 0;
        //    while (date >= DateTime.Now && DateTime.Now - date >= TimeSpan.FromDays(30))
        //    {
        //        date = date.AddDays(30);
        //        paymentNumber++;
        //    }
        //    foreach (var item1 in userPurchase.ProfitPurchases)
        //    {
        //        if (item1.ProductId.Equals(item.Id) && item1.UserId.Equals(userPurchase.UserId))
        //        {
        //            profit.Add(item1);
        //        }
        //    }
        //    profit.OrderBy(i => i.ForWhatPeriod);

        //    for (int i = 1; i <= paymentNumber; i++)
        //    {
        //        var profitMatch = profit.Where(i => i.ForWhatPeriod.Equals(i) &&
        //        i.UserId.Equals(userPurchase.UserId) && i.ProductId.Equals(item.Id));

        //        if (profitMatch == null)
        //        {
        //            notPaid++;
        //        }
        //        profit.AddRange(profitMatch);
        //    }
        //}
    }
}
