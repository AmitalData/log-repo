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

namespace Logitude.WarehouseLib.Data.EntityPOCOs
{
   
    public class WarehouseReleasePackage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("WarehouseRelease")]
        [Column("WarehouseReleaseId")]
	    public string WarehouseReleaseId { get; set; }
	      
        public virtual WarehouseRelease WarehouseRelease { get; set; }
        [Column("ContainerNumber")]
	    public string ContainerNumber { get; set; }
        [Column("Quantity")]
	    public int Quantity { get; set; }
        [Column("Weight")]
	    public decimal? Weight { get; set; }
        [Column("Volume")]
	    public decimal? Volume { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("PackageType")]
        [Column("PackageTypeId")]
	    public string PackageTypeId { get; set; }
	      
        public virtual PackageType PackageType { get; set; }
        [Column("Seal")]
	    public string Seal { get; set; }
        [Column("Harmonize")]
	    public string Harmonize { get; set; }
        [Column("Width")]
	    public double? Width { get; set; }
        [Column("Length")]
	    public double? Length { get; set; }
        [Column("Height")]
	    public double? Height { get; set; }
        [Column("IsContainer")]
	    public bool IsContainer { get; set; }
        [Column("VolumetricWeight")]
	    public double? VolumetricWeight { get; set; }
        [Column("Make")]
	    public string Make { get; set; }
        [Column("Model")]
	    public string Model { get; set; }
        [Column("Year")]
	    public string Year { get; set; }
        [Column("Color")]
	    public string Color { get; set; }
        [Column("ChassisNumber")]
	    public string ChassisNumber { get; set; }
        [Column("RegistrationNumber")]
	    public string RegistrationNumber { get; set; }
        [ForeignKey("Country")]
        [Column("CountryId")]
	    public string CountryId { get; set; }
	      
        public virtual Country Country { get; set; }
        [Column("IsUsed")]
	    public bool IsUsed { get; set; }
    }
}
	 