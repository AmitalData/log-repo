using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TicketTests.Models
{
    public class TicketPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TicketNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByContactId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CompanyId { get; set; }
        public string ContactId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string MainClassificationId { get; set; }
        public string OwnerId { get; set; }
        public string StageId { get; set; }
        public string SeverityId { get; set; }
        public string Subject { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsClosed { get; set; }
        public string SearchFields { get; set; }
        public string TicketDescription { get; set; }
        public string BusinessUnitId { get; set; }
        public string EmployeeGroupId { get; set; }
        public DateTime FirstResponseDue { get; set; }
        public DateTime ResolveWithinDue { get; set; }
        public string GuidId { get; set; }
        public int OpenEscalation { get; set; }
        public bool Closewithoutnotifying { get; set; }
        public string Source { get; set; }
        public string CreatedbyType { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsCreatedFromOutSide { get; set; }
        public bool IsResolveDue { get; set; }
        public bool IsResolveExamination { get; set; }
        public bool IsResponseDue { get; set; }
        public bool IsResponseExamination { get; set; }
        public string EmployeeGroupName { get; set; }
        public bool InternalMode { get; set; }
        public string ClassificationManager { get; set; }
        public string ClassificationNotify { get; set; }
        public string GroupManager { get; set; }
        public string SLAId { get; set; }
        public string EntityType { get; set; }
        public string SupportMailboxId { get; set; }
        public string LastCorrespondence { get; set; }
        public bool IsUpdateByAutomation { get; set; }
        public int ChangeSetOp { get; set; }
    }
}
