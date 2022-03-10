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
   
    public class SupplierInvoiceFreightAmount
    {
	 string dbms;

           [ForeignKey("SupplierInvoice")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual SupplierInvoice SupplierInvoice { get; set; }
        [ForeignKey("SupplierInvoice")]
        [Column("InvoiceCounterKey" ,Order = 2)]
	    public int InvoiceCounterKey { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CurrencyTypeCode" ,Order = 3)]
	    public string CurrencyTypeCode { get; set; }
        [Column("Amount")]
	    public decimal? Amount { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
    }
}
	 