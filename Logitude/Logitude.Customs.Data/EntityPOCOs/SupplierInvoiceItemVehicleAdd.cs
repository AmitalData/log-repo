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
   
    public class SupplierInvoiceItemVehicleAdd
    {
	 string dbms;

        [Key]
        [ForeignKey("SupplierInvoiceItemVehicle")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual SupplierInvoiceItemVehicle SupplierInvoiceItemVehicle { get; set; }
     [Key]
        [ForeignKey("SupplierInvoiceItemVehicle")]
        [Column("InvoiceCounterKey" ,Order = 2)]
	    public int InvoiceCounterKey { get; set; }
     [Key]
        [ForeignKey("SupplierInvoiceItemVehicle")]
        [Column("InvoiceItemLineNumber" ,Order = 3)]
	    public int InvoiceItemLineNumber { get; set; }
     [Key]
        [ForeignKey("SupplierInvoiceItemVehicle")]
        [Column("LineNumber" ,Order = 4)]
	    public int LineNumber { get; set; }
        [Column("VehicleModel")]
	    public string VehicleModel { get; set; }
        [Column("RichbitNumber")]
	    public string RichbitNumber { get; set; }
        [Column("ChassisNumber")]
	    public string ChassisNumber { get; set; }
        [Column("EngineNumber")]
	    public string EngineNumber { get; set; }
        [Column("WindowNumber")]
	    public string WindowNumber { get; set; }
        [Column("VehicleValue")]
	    public decimal? VehicleValue { get; set; }
        [Column("ChassisTax")]
	    public decimal? ChassisTax { get; set; }
        [Column("ChassisPurchaseTax")]
	    public decimal? ChassisPurchaseTax { get; set; }
        [Column("ChassisVat")]
	    public decimal? ChassisVat { get; set; }
        [Column("Exempt_type")]
	    public string Exempt_type { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 