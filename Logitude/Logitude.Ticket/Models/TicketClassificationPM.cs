using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TicketTests.Models
{
    public class TicketClassificationPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }
        public string ParentId { get; set; }
        public string DefaultSeverityId { get; set; }
        public string EmployeeGroupId { get; set; }
        public string ManagerUserId { get; set; }
        public string EscalationNotify { get; set; }
        public string ManagerUserEmail { get; set; }
    }
}
