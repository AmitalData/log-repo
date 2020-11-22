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
   
    public class CourierMaster
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDateTime")]
	    public DateTime? CreateDateTime { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime? UpdateDateTime { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("CustomsAirline")]
        [Column("AirlineId")]
	    public string AirlineId { get; set; }
	      
        public virtual CustomsAirline CustomsAirline { get; set; }
        [Column("MAWB")]
	    public string MAWB { get; set; }
        [Column("HAWB")]
	    public string HAWB { get; set; }
        [Column("EstimatedArrivalDate")]
	    public DateTime? EstimatedArrivalDate { get; set; }
        [ForeignKey("GatewayPort")]
        [Column("GatewayPortCode")]
	    public string GatewayPortCode { get; set; }
	      
        public virtual InternationalSite GatewayPort { get; set; }
        [ForeignKey("OriginPort")]
        [Column("OriginPortCode")]
	    public string OriginPortCode { get; set; }
	      
        public virtual InternationalSite OriginPort { get; set; }
        [Column("IsOpen")]
	    public bool IsOpen { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("User")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User User { get; set; }
        [ForeignKey("MAWBType")]
        [Column("MAWBTypeCode")]
	    public string MAWBTypeCode { get; set; }
	      
        public virtual MAWBType MAWBType { get; set; }
        [Column("ManifestNumber")]
	    public string ManifestNumber { get; set; }
        [Column("PackageQuantity")]
	    public int? PackageQuantity { get; set; }
        [Column("GrossMassMeasure")]
	    public decimal? GrossMassMeasure { get; set; }
        [Column("ShortHAWB")]
	    public string ShortHAWB { get; set; }
        [Column("FlightNumber")]
	    public string FlightNumber { get; set; }
        [Column("DepartureDate")]
	    public DateTime? DepartureDate { get; set; }
        [ForeignKey("FreightPaymentMethod")]
        [Column("WeightValueCode")]
	    public string WeightValueCode { get; set; }
	      
        public virtual FreightPaymentMethod FreightPaymentMethod { get; set; }
        [ForeignKey("DeliverySiteType")]
        [Column("StorageSiteCode")]
	    public string StorageSiteCode { get; set; }
	      
        public virtual DeliverySiteType DeliverySiteType { get; set; }
        [Column("TruckerId")]
	    public string TruckerId { get; set; }
        [ForeignKey("Card")]
        [Column("IntegratorCode")]
	    public string IntegratorCode { get; set; }
	      
        public virtual Card Card { get; set; }
        [Column("IsReadyForInvoice")]
	    public bool IsReadyForInvoice { get; set; }
        [Column("NoOfCourierHawb")]
	    public string NoOfCourierHawb { get; set; }
        [Column("IsAutomaticManifestSent")]
	    public bool IsAutomaticManifestSent { get; set; }
        [Column("PackageQuantityInMAWB")]
	    public int? PackageQuantityInMAWB { get; set; }
        [Column("LandingDate")]
	    public DateTime? LandingDate { get; set; }
        [Column("UnifreightLeadingFile")]
	    public string UnifreightLeadingFile { get; set; }
        [Column("CourierMasterRemarks")]
	    public string CourierMasterRemarks { get; set; }
    }
}
	 