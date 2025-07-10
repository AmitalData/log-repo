using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactMobileDevice
    {


        [Key]
        public string DeviceId { get; set; }
        public string NotificationUniqueKey { get; set; }
        public string Email { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string Devicetype { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsSignOut { get; set; }

        public string AppVersion { get; set; }




    }
}
