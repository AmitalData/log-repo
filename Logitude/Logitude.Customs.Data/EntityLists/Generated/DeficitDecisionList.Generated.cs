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
   public partial class DeficitDecisionList
   {
   
       [Key]
       [DataMember]
       public string DeficitId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? RequestDate  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string RequestID  { get; set; }
       [DataMember]
       public string RequestTypeCode  { get; set; }
       [DataMember]
       public string RequestTypeName  { get; set; }
       [DataMember]
       public string ApprovedProfessionCode  { get; set; }
       [DataMember]
       public string ApprovedProfessionName  { get; set; }
       [DataMember]
       public string DecisionCode  { get; set; }
       [DataMember]
       public string DecisionName  { get; set; }
       [DataMember]
       public string DecisionNoteForLetter  { get; set; }
       [DataMember]
       public decimal? TotalComponentAmount  { get; set; }
       [DataMember]
       public decimal? TotalEstimatedAmount  { get; set; }
       [DataMember]
       public decimal? TotalFinancialPenaltyAmount  { get; set; }
       [DataMember]
       public decimal? TotalInterestAmount  { get; set; }
       [DataMember]
       public decimal? TotalLinkingAmount  { get; set; }
   }

}
	 