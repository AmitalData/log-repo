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
        [Column("LastMileStatusCode")]
	    public string LastMileStatusCode { get; set; }
        [Column("LastMileStatusDate")]
	    public DateTime? LastMileStatusDate { get; set; }
        [Column("LastMileStatusRemarks")]
	    public string LastMileStatusRemarks { get; set; }
        [ForeignKey("MamanStatus")]
        [Column("StorageSiteStatusCode")]
	    public string StorageSiteStatusCode { get; set; }
	      
        public virtual MamanStatus MamanStatus { get; set; }
        [Column("StorageSiteErrorText")]
	    public string StorageSiteErrorText { get; set; }
        [Column("CourierPendingReasonList")]
	    public string CourierPendingReasonList { get; set; }
        [Column("LastMileStatusName")]
	    public string LastMileStatusName { get; set; }
        [Column("Delivered")]
	    public bool Delivered { get; set; }
        [ForeignKey("Trucker")]
        [Column("TruckerId")]
	    public string TruckerId { get; set; }
	      
        public virtual Trucker Trucker { get; set; }
        [Column("DistributionArea")]
	    public string DistributionArea { get; set; }
        [Column("CrateNumber")]
	    public string CrateNumber { get; set; }
        [Column("TerminalReleaseDate")]
	    public DateTime? TerminalReleaseDate { get; set; }
        [Column("LastMileServiceType")]
	    public string LastMileServiceType { get; set; }
        [ForeignKey("Card")]
        [Column("ShopId")]
	    public string ShopId { get; set; }
	      
        public virtual Card Card { get; set; }
    }
}
	 