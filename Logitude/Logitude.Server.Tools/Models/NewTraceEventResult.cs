using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Models
{
    public class NewTraceEventResult
    {
        public string EntityId { get; set; }
        public bool StatusChanged { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string StatusId { get; set; }
        public string OperationalStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusLocation { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? LastStatusLogDate { get; set; }
        public string LastSharedEventId { get; set; }
        public string LastSharedEventLocation { get; set; }
        public string LastSharedEventNotes { get; set; }
        public DateTime? LastSharedEventDate { get; set; }
    }
}
