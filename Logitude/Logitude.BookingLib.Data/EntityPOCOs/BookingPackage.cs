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
   
    public class BookingPackage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Booking")]
        [Column("BookingId")]
	    public string BookingId { get; set; }
	      
        public virtual Booking Booking { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("PackageType")]
        [Column("PackageTypeId")]
	    public string PackageTypeId { get; set; }
	      
        public virtual PackageType PackageType { get; set; }
        [Column("ContainerNumber")]
	    public string ContainerNumber { get; set; }
        [Column("Seal")]
	    public string Seal { get; set; }
        [Column("Quantity")]
	    public int? Quantity { get; set; }
        [Column("Weight")]
	    public decimal? Weight { get; set; }
        [Column("Volume")]
	    public decimal? Volume { get; set; }
        [Column("Tare")]
	    public decimal? Tare { get; set; }
        [Column("Height")]
	    public decimal? Height { get; set; }
        [Column("Width")]
	    public decimal? Width { get; set; }
        [Column("Length")]
	    public decimal? Length { get; set; }
        [Column("UnNumber")]
	    public string UnNumber { get; set; }
        [Column("ClassNumber")]
	    public string ClassNumber { get; set; }
        [Column("Temperature")]
	    public decimal? Temperature { get; set; }
        [Column("Ventilation")]
	    public decimal? Ventilation { get; set; }
        [Column("Seal2")]
	    public string Seal2 { get; set; }
        [Column("SOC")]
	    public int? SOC { get; set; }
        [Column("MarksAndNumbers")]
	    public string MarksAndNumbers { get; set; }
        [Column("PackagingGroup")]
	    public string PackagingGroup { get; set; }
        [Column("IMDGCode")]
	    public string IMDGCode { get; set; }
        [Column("FlashPoint")]
	    public string FlashPoint { get; set; }
        [Column("Harmonize")]
	    public string Harmonize { get; set; }
        [Column("MaterialDescription")]
	    public string MaterialDescription { get; set; }
        [Column("IsDangerous")]
	    public bool IsDangerous { get; set; }
        [Column("OriginalBookingPackageId")]
	    public string OriginalBookingPackageId { get; set; }
        [Column("VolumetricWeight")]
	    public decimal? VolumetricWeight { get; set; }
        [ForeignKey("ShipmentCommodity")]
        [Column("CommodityId")]
	    public string CommodityId { get; set; }
	      
        public virtual ShipmentCommodity ShipmentCommodity { get; set; }
    }
}
	 