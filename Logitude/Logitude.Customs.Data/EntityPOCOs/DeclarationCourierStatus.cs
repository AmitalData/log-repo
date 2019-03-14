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
   
    public class DeclarationCourierStatus
    {
	 string dbms;

        [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CourierManifestStatusCode")]
	    public string CourierManifestStatusCode { get; set; }
        [Column("CourierDeclarationStatusCode")]
	    public string CourierDeclarationStatusCode { get; set; }
        [Column("CourierPaymentStatusCode")]
	    public string CourierPaymentStatusCode { get; set; }
        [Column("IsCourierMissingClassification")]
	    public bool IsCourierMissingClassification { get; set; }
        [Column("IsClosedForFollowUp")]
	    public bool IsClosedForFollowUp { get; set; }
        [Column("HighLowValue")]
	    public string HighLowValue { get; set; }
        [Column("DocumentStatusCode")]
	    public string DocumentStatusCode { get; set; }
        [Column("TotalInvoiceAmountInUSD")]
	    public decimal? TotalInvoiceAmountInUSD { get; set; }
        [ForeignKey("CourierPendingReason")]
        [Column("CourierPendingReasonCode")]
	    public string CourierPendingReasonCode { get; set; }
	      
        public virtual CourierPendingReason CourierPendingReason { get; set; }
        [Column("PendingRemarks")]
	    public string PendingRemarks { get; set; }
        [Column("SpecialActionStatus")]
	    public string SpecialActionStatus { get; set; }
        [Column("FastIndividualProcessCode")]
	    public string FastIndividualProcessCode { get; set; }
        [Column("ManualProcessCode")]
	    public string ManualProcessCode { get; set; }
        [Column("TerminalSuspentionNumber")]
	    public string TerminalSuspentionNumber { get; set; }
    }
}
	 