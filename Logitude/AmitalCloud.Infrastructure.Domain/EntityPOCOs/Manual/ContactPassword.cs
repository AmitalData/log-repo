using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactPassword
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool MustChangePassword { get; set; }
        public bool IsLocked { get; set; }
        public int NumberOfRetries { get; set; }
        public DateTime? LockDateTime { get; set; }

        public bool SharedMobileAppAlertsforFollowedShipment { get; set; }
        public bool SharedMobileAppAlertonExceptions { get; set; }
        public bool IsSendNotificationForMobile { get; set; }

        public bool IsBCrypt { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }

        public string CaptchaKey { get; set; }
         
    }
}
