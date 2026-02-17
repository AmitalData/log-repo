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
   
    public class ClaimsRelatedEntitiesRefund
    {
	 string dbms;

        [Key]
        [ForeignKey("ClaimsRelatedEntity")]
        [Column("ClaimId" ,Order = 1)]
	    public string ClaimId { get; set; }
	      
        public virtual ClaimsRelatedEntity ClaimsRelatedEntity { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ClaimsRelatedEntity")]
        [Column("CounterKey" ,Order = 2)]
	    public int CounterKey { get; set; }
     [Key]
        [Column("RefundQuntityLineNo" ,Order = 3)]
	    public int RefundQuntityLineNo { get; set; }
        [Column("InvoiceNumber")]
	    public int? InvoiceNumber { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
        [Column("RefundQuntity")]
	    public decimal? RefundQuntity { get; set; }
    }
}
	 