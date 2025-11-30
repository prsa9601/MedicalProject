namespace MedicalProject.Models.Notification
{
    public class CreateListCommand 
    {
        public string Title { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();
        public bool SendToAll { get; set; } 
        public string Description { get; set; }
        public string Link { get; set; }
    }
    public class CreateNotificationCommand 
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
    public class EditNotificationCommand 
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; }
        public List<Guid> UserId { get; set; } = new List<Guid>();
        public string Description { get; set; }
        public string? Link { get; set; }
        public bool SendToAll { get; set; }
    }


    public class NotificationDto : BaseDto
    {
        public string Title { get; set; }
        public List<UserNotificationDto> Users { get; set; } = new List<UserNotificationDto>();
        public string Description { get; set; }
        public string? Link { get; set; }
    }
    public class UserNotificationDto : BaseDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class NotificationDtoForUser : BaseDto
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
    public class NotificationFilterParam : BaseFilterParam
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class NotificationFilterParamForUser : BaseFilterParam
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public required Guid UserId { get; set; }
    }
    public class NotificationFilterResultForUser : BaseFilter<NotificationDtoForUser, NotificationFilterParamForUser>
    {

    }
    public class NotificationFilterResult : BaseFilter<NotificationDto, NotificationFilterParam>
    {

    }
}
