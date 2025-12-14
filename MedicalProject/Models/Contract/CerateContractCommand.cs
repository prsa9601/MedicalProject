namespace MedicalProject.Models.Contract
{
    public class ContractDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContractStatus Status { get; set; }
    }
    
    public class ContractFilterParam : BaseFilterParam
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public ContractStatus Status { get; set; }
    }
    
    public class ContractFilterResult : BaseFilter<ContractDto, ContractFilterParam>
    {
    }
    
    public class CreateContractCommand
    {

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContractStatus Status { get; set; }
    }
    
    public class ContractAnsweredCommand
    {
        public Guid id { get; set; }
    }
    
    public enum ContractStatus
    {
        Answered,
        New
    }
}
