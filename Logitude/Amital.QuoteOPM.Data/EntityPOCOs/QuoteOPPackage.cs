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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPPackage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("QuoteOP")]
        [Column("QuoteOPId")]
	    public string QuoteOPId { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
        [ForeignKey("PackageType")]
        [Column("PackageTypeId")]
	    public string PackageTypeId { get; set; }
	      
        public virtual PackageType PackageType { get; set; }
        [Column("Quantity")]
	    public int? Quantity { get; set; }
        [Column("GrossWeight")]
	    public double? GrossWeight { get; set; }
        [Column("Volume")]
	    public double? Volume { get; set; }
        [Column("Height")]
	    public double? Height { get; set; }
        [Column("Width")]
	    public double? Width { get; set; }
        [Column("Length")]
	    public double? Length { get; set; }
        [Column("VolumetricWeight")]
	    public double? VolumetricWeight { get; set; }
    }
}
	 