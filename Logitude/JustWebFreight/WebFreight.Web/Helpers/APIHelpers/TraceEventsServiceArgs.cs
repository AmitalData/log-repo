using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class TraceEventsServiceArgs
    {
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string EventTypeId { get; set; }
        public string TraceEventId { get; set; }
        public string Notes { get; set; }
        public DateTime? EventDate { get; set; }
        public bool IsExternal { get; set; }
    }
}