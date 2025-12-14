namespace MedicalProject.Models.Order
{
    public class IsFinallyOrderCommand
    {
        public Guid orderId { get; set; }
        public Guid userId { get; set; }
    }
    public class SetOrderItemCommandViewModel
    {
        //public Guid orderId { get; set; }
        public Guid productId { get; set; }
        public decimal dongAmount { get; set; }
    }
    public class OrderIsFinallyViewModel
    {
        public Guid orderId { get; set; }
    }
}
