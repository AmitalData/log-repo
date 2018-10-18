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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class ExternalReconciliationLine
    {
	 string dbms;

        [Key]
        [ForeignKey("ExternalReconciliation")]
        [Column("ReconciliationId" ,Order = 1)]
	    public string ReconciliationId { get; set; }
	      
        public virtual ExternalReconciliation ExternalReconciliation { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [ForeignKey("LedgerTransaction")]
        [Column("LedgerTransactionId")]
	    public string LedgerTransactionId { get; set; }
	      
        public virtual LedgerTransaction LedgerTransaction { get; set; }
        [Column("GroupNumber")]
	    public int GroupNumber { get; set; }
        [ForeignKey("ReconcileExternalPageLine")]
        [Column("ExternalPageLineId")]
	    public string ExternalPageLineId { get; set; }
	      
        public virtual ReconcileExternalPageLine ReconcileExternalPageLine { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 