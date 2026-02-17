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
   
    public class DeclarationPaymentProtest
    {
	 string dbms;

        [Key]
        [ForeignKey("DeclarationPayment")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual DeclarationPayment DeclarationPayment { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [ForeignKey("PaymentProtestType")]
        [Column("ProtestTypeCode")]
	    public string ProtestTypeCode { get; set; }
	      
        public virtual PaymentProtestType PaymentProtestType { get; set; }
        [Column("CustomsAgentExplanation")]
	    public string CustomsAgentExplanation { get; set; }
        [Column("InvoiceNumber")]
	    public string InvoiceNumber { get; set; }
        [Column("GoodsItemLineNumber")]
	    public decimal? GoodsItemLineNumber { get; set; }
        [Column("GoodsItemClassification")]
	    public string GoodsItemClassification { get; set; }
        [Column("AmountInDispute")]
	    public decimal? AmountInDispute { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("InvoiceCounterKey")]
	    public int? InvoiceCounterKey { get; set; }
    }
}
	 