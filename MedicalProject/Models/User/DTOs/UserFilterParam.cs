
using MedicalProject.Models.User.Enum;
using System.Reflection.PortableExecutable;

namespace MedicalProject.Models.User.DTOs
{
    public class UserFilterParam : BaseFilterParam
    {
        public bool? IsActive { get; set; }
        //public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        //public List<Guid>? UserIds { get; set; }

    }
    public class UserFilterForDocumentsParam : BaseFilterParam
    {
        public UserDocumentStatus? UserStatus { get; set; } = null;
        public bool? IsActive { get; set; }
        public string? UserName { get; set; }
        //public Guid? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        //public List<Guid>? UserIds { get; set; }

    }
}
