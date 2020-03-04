using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class TicketList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string TicketNumber  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string CreatedByContactId  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string CompanyId  { get; set; }
       [DataMember]
       public string ContactId  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string MainClassificationId  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string StageId  { get; set; }
       [DataMember]
       public string SeverityId  { get; set; }
       [DataMember]
       public string Subject  { get; set; }
       [DataMember]
       public string TicketTypeId  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public string Field1  { get; set; }
       [DataMember]
       public string Field2  { get; set; }
       [DataMember]
       public string Field3  { get; set; }
       [DataMember]
       public string Field4  { get; set; }
       [DataMember]
       public string Field5  { get; set; }
       [DataMember]
       public string Field6  { get; set; }
       [DataMember]
       public string Field7  { get; set; }
       [DataMember]
       public string Field8  { get; set; }
       [DataMember]
       public string Field9  { get; set; }
       [DataMember]
       public string Field10  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string TicketDescription  { get; set; }
       [DataMember]
       public string CompanyName  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string StageName  { get; set; }
       [DataMember]
       public string MainClassificationName  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
       [DataMember]
       public string SeverityName  { get; set; }
       [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public string ClassificationAdvanced  { get; set; }
       [DataMember]
       public string ContactPhone  { get; set; }
       [DataMember]
       public string NextActivityTypeCode  { get; set; }
       [DataMember]
       public string NextActivitySubject  { get; set; }
       [DataMember]
       public DateTime? NextActivityDate  { get; set; }
       [DataMember]
       public string LastCompletedActivityTypeCode  { get; set; }
       [DataMember]
       public string LastCompletedActivitySubject  { get; set; }
       [DataMember]
       public DateTime? LastCompletedActivityDate  { get; set; }
       [DataMember]
       public string CCs  { get; set; }
       [DataMember]
       public string Bcc  { get; set; }
       [DataMember]
       public string CreatedByContactName  { get; set; }
       [DataMember]
       public string RankCode  { get; set; }
       [DataMember]
       public bool ActivityWatch  { get; set; }
       [DataMember]
       public bool HasOpenedActivities  { get; set; }
       [DataMember]
       public bool HasOpenedAppointments  { get; set; }
       [DataMember]
       public bool HasOpenedTasks  { get; set; }
       [DataMember]
       public bool HasOpenedPhonecalls  { get; set; }
       [DataMember]
       public bool ClosedTicketsWithOpenActivity  { get; set; }
       [DataMember]
       public string RankId  { get; set; }
       [DataMember]
       public int SeverityPriority  { get; set; }
       [DataMember]
       public string BusinessUnitId  { get; set; }
       [DataMember]
       public string EmployeeGroupId  { get; set; }
       [DataMember]
       public DateTime? FirstResponseTime  { get; set; }
       [DataMember]
       public DateTime? FirstResponseDue  { get; set; }
       [DataMember]
       public DateTime? FullResolvedTime  { get; set; }
       [DataMember]
       public DateTime? ResolveWithinDue  { get; set; }
       [DataMember]
       public string ContactEmail  { get; set; }
       [DataMember]
       public string GuidId  { get; set; }
       [DataMember]
       public int OpenEscalation  { get; set; }
       [DataMember]
       public string InternalUsers  { get; set; }
       [DataMember]
       public string ClosureDescription  { get; set; }
       [DataMember]
       public bool Closewithoutnotifying  { get; set; }
       [DataMember]
       public string StageCode  { get; set; }
       [DataMember]
       public string LastCompletedActivityTypeName  { get; set; }
       [DataMember]
       public string NextActivityTypeName  { get; set; }
       [DataMember]
       public string MyUnassignedTickets  { get; set; }
       [DataMember]
       public string MyAllOpenTickets  { get; set; }
       [DataMember]
       public string MySolvedTickets  { get; set; }
       [DataMember]
       public string MySLAFailures  { get; set; }
       [DataMember]
       public string SecondaryClassificationId  { get; set; }
       [DataMember]
       public string SecondaryClassificationName  { get; set; }
       [DataMember]
       public string ShipmentId  { get; set; }
       [DataMember]
       public string ShipmentNumber  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string MyRecentlyUpdatedTickets  { get; set; }
       [DataMember]
       public DateTime? FirstResolveDate  { get; set; }
       [DataMember]
       public string SeverityCode  { get; set; }
       [DataMember]
       public DateTime? TicketFirstResponseTime  { get; set; }
       [DataMember]
       public DateTime? TicketFirstResolveTime  { get; set; }
       [DataMember]
       public string MySolvedSLATickets  { get; set; }
       [DataMember]
       public string Source  { get; set; }
       [DataMember]
       public string CreatedbyType  { get; set; }
       [DataMember]
       public string SourceName  { get; set; }
       [DataMember]
       public string CreatedbyTypeName  { get; set; }
       [DataMember]
       public DateTime? FirstCloseDate  { get; set; }
       [DataMember]
       public DateTime? LastCloseDate  { get; set; }
       [DataMember]
       public DateTime? OpenDate  { get; set; }
       [DataMember]
       public int? OpenPeriodMinutes  { get; set; }
       [DataMember]
       public bool IsResolveDue  { get; set; }
       [DataMember]
       public string ResolveColor  { get; set; }
       [DataMember]
       public bool IsResolveExamination  { get; set; }
       [DataMember]
       public bool IsResponseDue  { get; set; }
       [DataMember]
       public string ResponseColor  { get; set; }
       [DataMember]
       public bool IsResponseExamination  { get; set; }
       [DataMember]
       public string CompanyTableName  { get; set; }
       [DataMember]
       public string RankName  { get; set; }
       [DataMember]
       public string LastStageName  { get; set; }
       [DataMember]
       public string LastActivityByUserName  { get; set; }
       [DataMember]
       public string LastActivityTypeName  { get; set; }
       [DataMember]
       public DateTime? LastActivityDate  { get; set; }
       [DataMember]
       public string OwnerEmail  { get; set; }
       [DataMember]
       public string EmployeeGroupName  { get; set; }
       [DataMember]
       public bool InternalMode  { get; set; }
       [DataMember]
       public string CustomerContactId  { get; set; }
       [DataMember]
       public string QuoteId  { get; set; }
       [DataMember]
       public string QuoteNumber  { get; set; }
       [DataMember]
       public string ContactTel  { get; set; }
       [DataMember]
       public string SLAName  { get; set; }
       [DataMember]
       public string SLAId  { get; set; }
       [DataMember]
       public string EntityType  { get; set; }
       [DataMember]
       public string EntityNumber  { get; set; }
       [DataMember]
       public string SupportMailboxId  { get; set; }
       [DataMember]
       public string LastCorrespondence  { get; set; }
   }

}
	 