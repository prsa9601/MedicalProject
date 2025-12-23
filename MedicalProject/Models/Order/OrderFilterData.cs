namespace MedicalProject.Models.Order
{
    public class OrderFilterData : BaseDto
    {
        public DateTime DateOfPurchase { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus status { get; set; }
        public OrderItemDto? OrderItems { get; set; }
    }
    public class OrderFilterParam : BaseFilterParam
    {
        public OrderFilter? OrderFilter { get; set; }
        public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public enum OrderFilter
    {
        None,
        HightPrice,
        LowPrice
    }
    public class OrderFilterResult : BaseFilter<OrderFilterData, OrderFilterParam>
    {
    }
}
