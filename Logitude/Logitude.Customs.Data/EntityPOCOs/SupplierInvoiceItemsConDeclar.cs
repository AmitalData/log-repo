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
   
    public class SupplierInvoiceItemsConDeclar
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
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("DeclarationNumber")]
	    public string DeclarationNumber { get; set; }
        [Column("ItemSequence")]
	    public int? ItemSequence { get; set; }
        [ForeignKey("LeadDocumentType")]
        [Column("DeclarationTypeCode")]
	    public string DeclarationTypeCode { get; set; }
	      
        public virtual LeadDocumentType LeadDocumentType { get; set; }
        [Column("InvoiceNumber")]
	    public int? InvoiceNumber { get; set; }
        [Column("Quantity")]
	    public decimal? Quantity { get; set; }
        [ForeignKey("MeasurmentUnit")]
        [Column("QuantityTypeCode")]
	    public string QuantityTypeCode { get; set; }
	      
        public virtual MeasurmentUnit MeasurmentUnit { get; set; }
    }
}
	 