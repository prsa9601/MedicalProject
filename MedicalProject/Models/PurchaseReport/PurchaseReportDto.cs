using MedicalProject.Models.Order;
using MedicalProject.Models.Product.DTOs;

namespace MedicalProject.Models.PurchaseReport
{
    public class PurchaseReportDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        //اطلاعات کل خرید آن زمان
        public string? TotalPrice { get; set; }
        public string? TotalDang { get; set; }
        public string? TotalProfit { get; set; }
        //اطلاعات خریداری شده
        public string? PurchasePrice { get; set; }
        public decimal PurchaseDang { get; set; }
        public string? Profit { get; set; }
        //اطلاعات اون موقع بر اساس یک دانگ
        public string? ProfitPerDang { get; set; }
        public string? PurchasePricePerDang { get; set; }
        public decimal PurchaseDangPerDang { get; set; }
    }
    public class ProductPurchaseReportDto : BaseDto
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public Guid PurchaseId { get; set; }
        public InventoryDto? InventoryDto { get; set; }
    }
    public class PurchaseReportUserInvestmentFilterResult : BaseFilter<UserPurchaseReportDto, UserPurchaseReportFilterParam>
    {

    }
    public class UserPurchaseReportDto : BaseDto
    {
        public Guid? UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Lastame { get; set; }
        public string? ImageName { get; set; }
        public int? InvestmentCount { get; set; }
        public List<OrderDto>? OrderDtos{ get; set; }
        public List<ProductPurchaseReportDto>? ProductPurchase { get; set; } = new();
        public List<PurchaseReportDto>? PurchaseReport { get; set; } = new();
        public List<ProfitPurchaseReportDto>? ProfitPurchases { get; set; } = new();

        //public UserBankAccountDto BankAccount { get; set; }
    }

    public class ProfitPurchaseReportDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public ProfitStatus Status { get; set; }
        public string ImageName { get; set; }
        public string ProductName { get; set; }
        public DateTime ForWhatTime { get; set; }
        public int ForWhatPeriod { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentTime ProfitableTime { get; set; }
    }

    public enum ProfitStatus
    {
        None = 0,
        UnSuccessful = 1,
        Success = 2,
    }

    public class PurchaseReportFilterParam : BaseFilterParam
    {
        public PurchaseReportFilter? PurchaseReportFilter { get; set; }
        public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class UserPurchaseReportFilterParam : BaseFilterParam
    {
        public PurchaseReportFilter? PurchaseReportFilter { get; set; }
        public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
 
    public class UserPurchaseReportForCurrentUserFilterParam : BaseFilterParam
    {
        public PurchaseReportFilter? PurchaseReportFilter { get; set; }
        public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public enum PurchaseReportFilter
    {
        None,
        HightPrice,
        LowPrice
    }

    public class PurchaseReportFilterResult : BaseFilter<PurchaseReportDto, PurchaseReportFilterParam>
    {

    }


    public class UserProfitPurchaseReportDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string ImageName { get; set; }
        public string Lastame { get; set; }
        public int InvestmentCount { get; set; }
        public List<OrderProfitDto>? OrderDtos { get; set; } = new();

        //public UserBankAccountDto BankAccount { get; set; }

    }
    public class UserProfitPurchaseReportDtoFilterParam : BaseFilterParam
    {
        public string? search { get; set; }
        public bool? IsDebtor { get; set; }
    }
    public class UserProfitPurchaseReportDtoFilterResult : BaseFilter<UserProfitPurchaseReportDto, UserProfitPurchaseReportDtoFilterParam>
    {
    }
    public class OrderProfitDto : BaseDto
    {
        public DateTime DateOfPurchase { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus status { get; set; }
        public OrderProfitItemDto OrderItems { get; set; } = new();
    }
    public class OrderProfitItemDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public ProductProfitPurchaseReportDto Product { get; set; }
        //قیمت هر دانگ
        public string PricePerDong { get; set; }
        //مقدار خواسته شده
        public decimal DongAmount { get; set; }
        public Guid InventoryId { get; set; }

        public decimal TotalPrice
        {
            get
            {
                if (decimal.TryParse(PricePerDong, out var price))
                {
                    return DongAmount * price;
                }
                return 0;
            }
        }
    }
    public class ProductProfitPurchaseReportDto : BaseDto
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public InventoryDto InventoryDto { get; set; }
        public List<PaidProfit> Paid { get; set; }
        public List<UnpaidProfit> Unpaid { get; set; }
        public List<PurchaseProfitReportDto> purchaseReportDto { get; set; } = new();
        public List<ProfitPurchaseReportProfitDto> profitPurchaseReportDtos { get; set; } = new();
    }
    public class PurchaseProfitReportDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        //اطلاعات کل خرید آن زمان
        public string TotalPrice { get; set; }
        public string TotalDang { get; set; }
        public string TotalProfit { get; set; }
        //اطلاعات خریداری شده
        public string PurchasePrice { get; set; }
        public decimal PurchaseDang { get; set; }
        public string Profit { get; set; }
        //اطلاعات اون موقع بر اساس یک دانگ
        public string ProfitPerDang { get; set; }
        public string PurchasePricePerDang { get; set; }
        public decimal PurchaseDangPerDang { get; set; }
    }
    public class ProfitPurchaseReportProfitDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public ProfitStatus Status { get; set; }
        public DateTime ForWhatTime { get; set; }
        public int ForWhatPeriod { get; set; }
        public string ImageName { get; set; }
        public string ProductName { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentTime ProfitableTime { get; set; }
    }
    public class UnpaidProfit
    {
        public int PeriodNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ExpectedAmount { get; set; }
    }
    public class PaidProfit
    {
        public int PeriodNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ExpectedAmount { get; set; }
    }
}
