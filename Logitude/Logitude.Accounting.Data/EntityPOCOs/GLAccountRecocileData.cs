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
   
    public class GLAccountRecocileData
    {
	 string dbms;

        [Key]
        [ForeignKey("GLAccount")]
        [Column("AccountId" ,Order = 1)]
	    public string AccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("LastReconciledByUser")]
        [Column("LastReconciledByUserId")]
	    public string LastReconciledByUserId { get; set; }
	      
        public virtual User LastReconciledByUser { get; set; }
        [Column("LastReconcileDateTime")]
	    public DateTime? LastReconcileDateTime { get; set; }
    }
}
	 