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
   public partial class ActivityList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ActivityTypeCode  { get; set; }
       [DataMember]
       public string Subject  { get; set; }
       [DataMember]
       public DateTime? DueDate  { get; set; }
       [DataMember]
       public string PriorityCode  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public DateTime? StartDateTime  { get; set; }
       [DataMember]
       public DateTime? EndDateTime  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string CallTypeCode  { get; set; }
       [DataMember]
       public string CallPurpose  { get; set; }
       [DataMember]
       public string CallDetails  { get; set; }
       [DataMember]
       public string CallResult  { get; set; }
       [DataMember]
       public string PhoneNumber  { get; set; }
       [DataMember]
       public int? Duration  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string ActivityStatusCode  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string BranchId  { get; set; }
       [DataMember]
       public bool AllDayEvent  { get; set; }
       [DataMember]
       public string ActivityTimeTypeCode  { get; set; }
       [DataMember]
       public string Location  { get; set; }
       [DataMember]
       public string ActivityTypeName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ActivityStatusName  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string PriorityName  { get; set; }
       [DataMember]
       public bool IsLeftVoiceMail  { get; set; }
       [DataMember]
       public string OutlookId  { get; set; }
       [DataMember]
       public bool NeedSynchronization  { get; set; }
       [DataMember]
       public bool IsMarkedCompleted  { get; set; }
       [DataMember]
       public bool IsOpen  { get; set; }
       [DataMember]
       public string MeetingSummary  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string OpportunityId  { get; set; }
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
       public DateTime? CompleteDate  { get; set; }
       [DataMember]
       public string OpportunitySubject  { get; set; }
       [DataMember]
       public string BusinessUnitId  { get; set; }
       [DataMember]
       public string BusinessUnitName  { get; set; }
       [DataMember]
       public string SenderEmail  { get; set; }
       [DataMember]
       public string CommunicationLogId  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string SenderContactId  { get; set; }
       [DataMember]
       public string CallWithId  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string QuoteId  { get; set; }
       [DataMember]
       public string QuoteNumber  { get; set; }
       [DataMember]
       public string TicketId  { get; set; }
       [DataMember]
       public DateTime? SortingDate  { get; set; }
       [DataMember]
       public string SortingBy  { get; set; }
       [DataMember]
       public DateTime? SendReceiveDate  { get; set; }
       [DataMember]
       public string ActivityWith  { get; set; }
       [DataMember]
       public bool DescriptionRightToLeft  { get; set; }
       [DataMember]
       public bool MeetingSummaryRightToLeft  { get; set; }
       [DataMember]
       public DateTime? UpcomingDate  { get; set; }
       [DataMember]
       public DateTime? SortByDate  { get; set; }
       [DataMember]
       public string SenderContactName  { get; set; }
       [DataMember]
       public string OriginalActivitySubject  { get; set; }
       [DataMember]
       public bool IsHybrid  { get; set; }
       [DataMember]
       public bool IsCustomerBlockedBusinessUnit  { get; set; }
       [DataMember]
       public bool IsCopy  { get; set; }
       [DataMember]
       public byte[] LastModified  { get; set; }
       [DataMember]
       public bool DontSetNeedSynchronization  { get; set; }
       [DataMember]
       public string ActivityTypePathCode  { get; set; }
       [DataMember]
       public string ObjectTableName  { get; set; }
       [DataMember]
       public bool PostToFollowers  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string RecipientsEmails  { get; set; }
       [DataMember]
       public string From  { get; set; }
       [DataMember]
       public string To  { get; set; }
       [DataMember]
       public string Cc  { get; set; }
       [DataMember]
       public DateTime? ArchiveDate  { get; set; }
       [DataMember]
       public string CallWithName  { get; set; }
       [DataMember]
       public int? DueDateOffset  { get; set; }
       [DataMember]
       public string DueDateDateField  { get; set; }
       [DataMember]
       public string BusinessProcessQueueName  { get; set; }
       [DataMember]
       public string TeamName  { get; set; }
   }

}
	 