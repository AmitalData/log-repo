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
   
    public class SupplierInvoiceItemsLevy
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
        [ForeignKey("TradeLevyExamptType")]
        [Column("TradeLevyExamptCode")]
	    public string TradeLevyExamptCode { get; set; }
	      
        public virtual TradeLevyExamptType TradeLevyExamptType { get; set; }
        [Column("TradeLevyNumber")]
	    public string TradeLevyNumber { get; set; }
    }
}
	 