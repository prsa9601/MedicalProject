namespace MedicalProject.Models.Order
{
    public class OrderDto : BaseDto
    {
        public DateTime DateOfPurchase { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
    public class OrderItemDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        //قیمت هر دانگ
        public string PricePerDong { get; set; }
        //مقدار خواسته شده
        public int DongAmount { get; set; }
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

    public enum OrderStatus
    {
        AwaitingPayment,
        paid
    }
}
