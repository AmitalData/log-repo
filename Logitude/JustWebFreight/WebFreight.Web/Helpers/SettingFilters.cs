using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SettingFilters
    {
  

        public bool SharedMobileAppAlertonExceptions { get; set; }
        public bool SharedMobileAppAlertsforFollowedShipment { get; set; }
        public bool IsSendNotificationForMobile { get; set; }
        public string Email { get; set; }
        public string ContactId { get; set; }
    }
}