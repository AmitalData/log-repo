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
 
namespace Logitude.BookingLib.Data.EntityPOCOs
{
   
    public class FlightsSchedulesRequest
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("FromPort")]
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
	      
        public virtual Port FromPort { get; set; }
        [ForeignKey("ToPort")]
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
	      
        public virtual Port ToPort { get; set; }
        [ForeignKey("Airline")]
        [Column("AirlineId")]
	    public string AirlineId { get; set; }
	      
        public virtual Card Airline { get; set; }
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
        [Column("BookingId")]
	    public string BookingId { get; set; }
        [Column("ETD")]
	    public DateTime? ETD { get; set; }
        [Column("ETA")]
	    public DateTime? ETA { get; set; }
        [Column("Volume")]
	    public decimal? Volume { get; set; }
        [Column("GrossWeight")]
	    public decimal? GrossWeight { get; set; }
        [ForeignKey("VolumeUnit")]
        [Column("VolumeUnitCode")]
	    public string VolumeUnitCode { get; set; }
	      
        public virtual VolumeUnit VolumeUnit { get; set; }
        [ForeignKey("GrossWeightUnit")]
        [Column("GrossWeightUnitCode")]
	    public string GrossWeightUnitCode { get; set; }
	      
        public virtual WeightUnit GrossWeightUnit { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("ResponseDate")]
	    public DateTime? ResponseDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("Status")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual FlightsSchedulesRequestStatus Status { get; set; }
        [Column("AnswerOSI")]
	    public string AnswerOSI { get; set; }
        [Column("AnswerReasonForNoReply")]
	    public string AnswerReasonForNoReply { get; set; }
        [Column("RequestDetails")]
	    public string RequestDetails { get; set; }
    }
}
	 