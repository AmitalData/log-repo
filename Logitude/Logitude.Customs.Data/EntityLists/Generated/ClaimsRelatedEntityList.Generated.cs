using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class ClaimsRelatedEntityList
   {
   
       [Key]
       [DataMember]
       public string ClaimId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int EntityCounterKey  { get; set; }
       [DataMember]
       public string ClaimEntityTypeCode  { get; set; }
       [DataMember]
       public string ClaimEntityTypeName  { get; set; }
       [DataMember]
       public string ClaimEntityNumber  { get; set; }
       [DataMember]
       public string ExternalClaimNumber  { get; set; }
       [DataMember]
       public string CourtCode  { get; set; }
       [DataMember]
       public string CourtName  { get; set; }
       [DataMember]
       public string ProceedingNumber  { get; set; }
       [DataMember]
       public bool IsFinancialRefundDemand  { get; set; }
       [DataMember]
       public string SeconderyClaimEntityCode  { get; set; }
       [DataMember]
       public string SeconderyClaimEntityName  { get; set; }
       [DataMember]
       public string SeconderyClaimEntityID  { get; set; }
       [DataMember]
       public decimal? ClaimAmount  { get; set; }
       [DataMember]
       public decimal? DeclarationVersion  { get; set; }
       [DataMember]
       public string CommitteeDecisionNumber  { get; set; }
       [DataMember]
       public string AbandonmentDestructionReferenc  { get; set; }
       [DataMember]
       public string WarehouseTypeCode  { get; set; }
       [DataMember]
       public string WarehouseTypeName  { get; set; }
       [DataMember]
       public string ClaimExplanation  { get; set; }
       [DataMember]
       public string ContinuousMessagesTypeCode  { get; set; }
       [DataMember]
       public string ContinuousMessagesTypeName  { get; set; }
       [DataMember]
       public string ClaimRequestNumber  { get; set; }
       [DataMember]
       public string CustomsExceptions  { get; set; }
       [DataMember]
       public string TapagNumber  { get; set; }
       [DataMember]
       public int? Numeral  { get; set; }
       [DataMember]
       public string CustomsBranchCode  { get; set; }
       [DataMember]
       public bool IsSendClaimsRelatedEntity  { get; set; }
       [DataMember]
       public string DecisionCode  { get; set; }
       [DataMember]
       public string DecisionName  { get; set; }
       [DataMember]
       public string DecisionNote  { get; set; }
       [DataMember]
       public string EilatVatRefoundDecision  { get; set; }
       [DataMember]
       public decimal? DepositingAmount  { get; set; }
       [DataMember]
       public decimal? RefundAmount  { get; set; }
   }

}
	 