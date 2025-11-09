namespace MedicalProject.Models.Order
{
    public class IsFinallyOrderCommand
    {
        public Guid orderId { get; set; }
        public Guid userId { get; set; }
    }
}
