using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ContactMobileDeviceFilters  
    {

        public string Email { get; set; }
        public string DeviceId { get; set; }
       public string  NotificationUniqueKey { get; set; }
       public string AppVersion { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string Devicetype { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsSignOut { get; set; }
        
    }
}