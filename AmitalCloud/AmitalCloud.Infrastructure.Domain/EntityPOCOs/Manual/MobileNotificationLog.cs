using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class MobileNotificationLog
    {


        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string NotificationMessage { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string Exception { get; set; }
        public string Log { get; set; }
        public string IOSStatus { get; set; }
        public string AndroidStatus { get; set; }
        public int NumberOfRetriesIOS { get; set; }
        public int NumberOfRetriesAndroid { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsException { get; set; }

        public bool IsRead { get; set; }
        public bool IsDelete { get; set; }
        public string NotificationMessageIOS { get; set; }
        public string NotificationMessageAndroid { get; set; }
        public string XML { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? SourceEventDate { get; set; }
    }
}
