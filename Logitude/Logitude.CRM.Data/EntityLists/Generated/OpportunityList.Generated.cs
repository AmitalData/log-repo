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
   public partial class OpportunityList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string Subject  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string LeadSourceId  { get; set; }
       [DataMember]
       public string ContactId  { get; set; }
       [DataMember]
       public DateTime? EstimatedClosingDate  { get; set; }
       [DataMember]
       public string StageId  { get; set; }
       [DataMember]
       public int? Probability  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string RatingCode  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public DateTime? ActualClosingDate  { get; set; }
       [DataMember]
       public string ClosingDescription  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string StageName  { get; set; }
       [DataMember]
       public string RatingName  { get; set; }
       [DataMember]
       public string ClosedToCompetitorId  { get; set; }
       [DataMember]
       public int? NumberOfShipments  { get; set; }
       [DataMember]
       public decimal? ValueField  { get; set; }
       [DataMember]
       public DateTime? LastStageDate  { get; set; }
       [DataMember]
       public string LastStageIdBeforeClosure  { get; set; }
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
       public string CountryName  { get; set; }
       [DataMember]
       public DateTime? LastCompletedActivityDate  { get; set; }
       [DataMember]
       public string LeadDescription  { get; set; }
       [DataMember]
       public string LastCompletedActivityTypeCode  { get; set; }
       [DataMember]
       public string LastActivitySubject  { get; set; }
       [DataMember]
       public DateTime? NextActivityDate  { get; set; }
       [DataMember]
       public string LastCompletedActivityTypeName  { get; set; }
       [DataMember]
       public string IndustryName  { get; set; }
       [DataMember]
       public string NextActivityTypeCode  { get; set; }
       [DataMember]
       public string NextActivitySubject  { get; set; }
       [DataMember]
       public string NextActivityTypeName  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public DateTime? StageDueDate  { get; set; }
       [DataMember]
       public string BusinessUnitId  { get; set; }
       [DataMember]
       public string BusinessUnitName  { get; set; }
       [DataMember]
       public int? StageProbability  { get; set; }
       [DataMember]
       public int? RatingIndexOrder  { get; set; }
       [DataMember]
       public string LeadUserId  { get; set; }
       [DataMember]
       public string LeadPartnerId  { get; set; }
       [DataMember]
       public string AgentId  { get; set; }
       [DataMember]
       public string ForeignClientId  { get; set; }
       [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public string ContactPhone  { get; set; }
       [DataMember]
       public string ClosingReasonId  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string ClosingReasonName  { get; set; }
       [DataMember]
       public string LeadSourceName  { get; set; }
       [DataMember]
       public string ContactEmail  { get; set; }
       [DataMember]
       public bool ActivityWatch  { get; set; }
       [DataMember]
       public string OpportunityTypeId  { get; set; }
       [DataMember]
       public string OpportunityTypeName  { get; set; }
       [DataMember]
       public string LeadPartnerName  { get; set; }
       [DataMember]
       public bool IsClosedLost  { get; set; }
       [DataMember]
       public string CustomerRankCode  { get; set; }
       [DataMember]
       public string CustomerRankName  { get; set; }
       [DataMember]
       public bool PostToFollowersAsWon  { get; set; }
       [DataMember]
       public string ClosingReasonCode  { get; set; }
       [DataMember]
       public string CustomerExternalId  { get; set; }
       [DataMember]
       public bool IsCopy  { get; set; }
       [DataMember]
       public string CopyFromEntityId  { get; set; }
       [DataMember]
       public bool IsCustomerBlockedBusinessUnit  { get; set; }
   }

}
	 