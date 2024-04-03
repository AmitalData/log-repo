using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Params
{
    public class EmailMessageParams
    {
        public string Environment { get; set; }
        public string SiteUri { get; set; }
        public string SenderEmail { get; set; }
        public string TeamName { get; set; }
        public bool IsLogBox { get; set; }
        public bool IsCargoTracking { get; set; }

        public string TenantName { get; set; }
    }
}