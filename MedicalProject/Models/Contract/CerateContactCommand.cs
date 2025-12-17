namespace MedicalProject.Models.Contact
{
    public class ContactDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContactStatus Status { get; set; }
    }
    public class ContactFilterParam : BaseFilterParam
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public ContactStatus? Status { get; set; }
    }
    public class ContactFilterResult : BaseFilter<ContactDto, ContactFilterParam>
    {
    }
    public class CreateContactCommand
    {

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContactStatus Status { get; set; }
    }
    
    public class ContactAnsweredCommand
    {
        public Guid id { get; set; }
    }
    
    public enum ContactStatus
    {
        Answered,
        New
    }
}
