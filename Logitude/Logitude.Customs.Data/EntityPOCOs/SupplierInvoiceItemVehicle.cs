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
   
    public class SupplierInvoiceItemVehicle
    {
	 string dbms;

        [Key]
        [ForeignKey("SupplierInvoiceItem")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual SupplierInvoiceItem SupplierInvoiceItem { get; set; }
     [Key]
        [ForeignKey("SupplierInvoiceItem")]
        [Column("InvoiceCounterKey" ,Order = 2)]
	    public int InvoiceCounterKey { get; set; }
     [Key]
        [ForeignKey("SupplierInvoiceItem")]
        [Column("InvoiceItemLineNumber" ,Order = 3)]
	    public int InvoiceItemLineNumber { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 4)]
	    public int LineNumber { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
        [ForeignKey("VehicleType")]
        [Column("VehicleTypeCode")]
	    public string VehicleTypeCode { get; set; }
	      
        public virtual CargoIdentityQualifier VehicleType { get; set; }
        [Column("VehicleChassisNumber")]
	    public string VehicleChassisNumber { get; set; }
        [Column("RichbitFileNumber")]
	    public string RichbitFileNumber { get; set; }
        [ForeignKey("Vehicle")]
        [Column("VehicleId")]
	    public string VehicleId { get; set; }
	      
        public virtual Vehicle Vehicle { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("ExcludeFromInterface")]
	    public bool ExcludeFromInterface { get; set; }
    }
}
	 