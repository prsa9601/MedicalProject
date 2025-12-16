using MedicalProject.Models.PurchaseReport;

namespace MedicalProject.Models.Profit
{
    public class CreateProfitCommand
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public ProfitStatus Status { get; set; }
        public IFormFile Image { get; set; }
        public DateTime ForWhateTime { get; set; }
        public int ForWhatePeriod { get; set; }
    }
}
