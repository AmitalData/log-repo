using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class ClaimsRelatedEntity
    {
	 string dbms;

        [Key]
        [ForeignKey("Claim")]
        [Column("ClaimId" ,Order = 1)]
	    public string ClaimId { get; set; }
	      
        public virtual Claim Claim { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("EntityCounterKey" ,Order = 2)]
	    public int EntityCounterKey { get; set; }
        [ForeignKey("ClaimEntityType")]
        [Column("ClaimEntityTypeCode")]
	    public string ClaimEntityTypeCode { get; set; }
	      
        public virtual ClaimEntity ClaimEntityType { get; set; }
        [Column("ClaimEntityNumber")]
	    public string ClaimEntityNumber { get; set; }
        [Column("ExternalClaimNumber")]
	    public string ExternalClaimNumber { get; set; }
        [ForeignKey("CourtInstance")]
        [Column("CourtCode")]
	    public string CourtCode { get; set; }
	      
        public virtual CourtInstance CourtInstance { get; set; }
        [Column("ProceedingNumber")]
	    public string ProceedingNumber { get; set; }
        [Column("IsFinancialRefundDemand")]
	    public bool IsFinancialRefundDemand { get; set; }
        [ForeignKey("SeconderyClaimEntity")]
        [Column("SeconderyClaimEntityCode")]
	    public string SeconderyClaimEntityCode { get; set; }
	      
        public virtual ClaimEntity SeconderyClaimEntity { get; set; }
        [Column("SeconderyClaimEntityID")]
	    public string SeconderyClaimEntityID { get; set; }
        [Column("ClaimAmount")]
	    public decimal? ClaimAmount { get; set; }
        [Column("DeclarationVersion")]
	    public decimal? DeclarationVersion { get; set; }
        [Column("CommitteeDecisionNumber")]
	    public string CommitteeDecisionNumber { get; set; }
        [Column("AbandonmentDestructionReferenc")]
	    public string AbandonmentDestructionReferenc { get; set; }
        [ForeignKey("WarehouseType")]
        [Column("WarehouseTypeCode")]
	    public string WarehouseTypeCode { get; set; }
	      
        public virtual SiteLookup WarehouseType { get; set; }
        [Column("ClaimExplanation")]
	    public string ClaimExplanation { get; set; }
        [ForeignKey("ContinuousMessagesType")]
        [Column("ContinuousMessagesTypeCode")]
	    public string ContinuousMessagesTypeCode { get; set; }
	      
        public virtual ContinuousMessagesTypeCode ContinuousMessagesType { get; set; }
        [Column("ClaimRequestNumber")]
	    public string ClaimRequestNumber { get; set; }
        [Column("CustomsExceptions")]
	    public string CustomsExceptions { get; set; }
        [Column("TapagNumber")]
	    public string TapagNumber { get; set; }
        [Column("Numeral")]
	    public int? Numeral { get; set; }
        [ForeignKey("CustomsBranch")]
        [Column("CustomsBranchCode")]
	    public string CustomsBranchCode { get; set; }
	      
        public virtual CustomsHouseType CustomsBranch { get; set; }
        [ForeignKey("DecisionType")]
        [Column("DecisionCode")]
	    public string DecisionCode { get; set; }
	      
        public virtual DecisionType DecisionType { get; set; }
        [Column("DecisionNote")]
	    public string DecisionNote { get; set; }
        [Column("EilatVatRefoundDecision")]
	    public string EilatVatRefoundDecision { get; set; }
        [Column("DepositingAmount")]
	    public decimal? DepositingAmount { get; set; }
        [Column("RefundAmount")]
	    public decimal? RefundAmount { get; set; }
        [ForeignKey("ContinuousRequestType")]
        [Column("ContinuousRequestTypeCode")]
	    public string ContinuousRequestTypeCode { get; set; }
	      
        public virtual ContinuousRequestType ContinuousRequestType { get; set; }
        [Column("Explanation")]
	    public string Explanation { get; set; }
        [Column("Note")]
	    public string Note { get; set; }
    }
}
	 