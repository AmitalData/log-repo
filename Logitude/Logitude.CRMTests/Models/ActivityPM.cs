using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRMTests.Models
{
    public class ActivityPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ActivityTypeCode { get; set; }
        public string Subject { get; set; }
        public DateTime? DueDate { get; set; }
        public string PriorityCode { get; set; }
        public string OwnerId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Description { get; set; }
        public string CallTypeCode { get; set; }
        public string CallPurpose { get; set; }
        public string CallDetails { get; set; }
        public string CallResult { get; set; }
        public string PhoneNumber { get; set; }
        public int? Duration { get; set; }
        public string Notes { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ActivityStatusCode { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string BranchId { get; set; }
        public bool AllDayEvent { get; set; }
        public string ActivityTimeTypeCode { get; set; }
        public string Location { get; set; }
        public string ActivityTypeName { get; set; }
        public string SearchFields { get; set; }
        public string ActivityStatusName { get; set; }
        public string OwnerName { get; set; }
        public string PriorityName { get; set; }
        public bool IsLeftVoiceMail { get; set; }
        public string OutlookId { get; set; }
        public bool NeedSynchronization { get; set; }
        public bool IsMarkedCompleted { get; set; }
        public bool IsOpen { get; set; }
        public string MeetingSummary { get; set; }
        public string CustomerId { get; set; }
        public string OpportunityId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string OpportunitySubject { get; set; }
        public string BusinessUnitId { get; set; }
        public string CustomerName { get; set; }
        public string ConcurrencyGUID { get; set; }
        public object CreatedByUserName { get; set; }
        public DateTime SortingDate { get; set; }
        public string SortingBy { get; set; }
        public string ActivityWith { get; set; }
        public bool IsHybrid { get; set; }
        public bool IsCustomerBlockedBusinessUnit { get; set; }
        public bool IsCopy { get; set; }
        public bool DontSetNeedSynchronization { get; set; }
        public int ChangeSetOp { get; set; }
        public string CallWithId { get; set; }
    }
}
