using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EntityTrackingFilter
    {

        public string ShipmentId { get; set; }
        public string ContactId { get; set; }

        public DateTime TrackDate { get; set; }
        public bool IsDelete { get; set; }
        public string CustomFileId { get; set; }
    
    }
}