using MedicalProject.Models.Product.DTOs;

namespace MedicalProject.Models.Inventory
{
    public class AddInventoryCommand 
    {
        public string totalPrice { get; set; }
        public decimal dong { get; set; }
        public string profit { get; set; }
        public Guid productId { get; set; }
        public PaymentTime? paymentTime { get; set; }
    }
    public class EditInventoryCommand 
    {
        public string totalPrice { get; set; }
        public decimal dong { get; set; }
        public string profit { get; set; }
        public Guid productId { get; set; }
        public PaymentTime? paymentTime { get; set; }
    }
    public class SetInventoryProfitableCommand 
    {
        public Guid productId { get; set; }
        public PaymentTime paymentTime { get; set; }
    }
}
