using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ActivityFilters
    {
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public bool IsOpen { get; set; }
        public string OwnerId { get; set; }
        public bool IsMyActivities { get; set; }
        public string SearchField { get; set; }
    }
}