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
   
    public class CertificateOfOriginItem
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("CertificateOfOrigin")]
        [Column("CertificateOfOriginId")]
	    public string CertificateOfOriginId { get; set; }
	      
        public virtual CertificateOfOrigin CertificateOfOrigin { get; set; }
        [Column("ItemSerial")]
	    public string ItemSerial { get; set; }
        [Column("ItemId")]
	    public string ItemId { get; set; }
        [ForeignKey("OriginCriterion")]
        [Column("OriginCriterionCode")]
	    public string OriginCriterionCode { get; set; }
	      
        public virtual OriginCriterion OriginCriterion { get; set; }
        [Column("MarksAndNumbers")]
	    public string MarksAndNumbers { get; set; }
        [Column("PackageQuantity")]
	    public string PackageQuantity { get; set; }
        [ForeignKey("PackingType")]
        [Column("PackageType")]
	    public string PackageType { get; set; }
	      
        public virtual PackingType PackingType { get; set; }
        [Column("ItemDescription")]
	    public string ItemDescription { get; set; }
        [Column("Weight")]
	    public string Weight { get; set; }
        [ForeignKey("MeasurmentUnit")]
        [Column("MeasureType")]
	    public string MeasureType { get; set; }
	      
        public virtual MeasurmentUnit MeasurmentUnit { get; set; }
        [Column("InvoiceConnect")]
	    public string InvoiceConnect { get; set; }
        [Column("ContainerIsoCode")]
	    public string ContainerIsoCode { get; set; }
    }
}
	 