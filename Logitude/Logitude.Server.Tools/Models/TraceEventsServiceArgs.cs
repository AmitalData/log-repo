using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Models
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
        public bool IsFromAPI { get; set; }
        public string UserId { get; set; }
        public bool IsAutomation { get; set; }
        public object EntityPM { get; set; }
    }
}
