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
   
    public class SupplierInvoiceModification
    {
	 string dbms;

        [Key]
        [ForeignKey("SupplierInvoice")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual SupplierInvoice SupplierInvoice { get; set; }
     [Key]
        [ForeignKey("SupplierInvoice")]
        [Column("InvoiceCounterKey" ,Order = 2)]
	    public int InvoiceCounterKey { get; set; }
        [ForeignKey("ModificationAndDiscountType")]
        [Column("TypeCode")]
	    public string TypeCode { get; set; }
	      
        public virtual ModificationAndDiscountType ModificationAndDiscountType { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CurrencyType")]
        [Column("CurrencyTypeCode")]
	    public string CurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType CurrencyType { get; set; }
        [Column("Amount")]
	    public decimal? Amount { get; set; }
     [Key]
        [Column("ModificationCounterKey" ,Order = 3)]
	    public int ModificationCounterKey { get; set; }
    }
}
	 