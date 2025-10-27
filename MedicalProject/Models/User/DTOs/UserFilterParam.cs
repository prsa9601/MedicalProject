
namespace MedicalProject.Models.User.DTOs
{
    public class UserFilterParam : BaseFilterParam
    {
        public bool? IsActive { get; set; }
        //public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        //public List<Guid>? UserIds { get; set; }

    }
}
