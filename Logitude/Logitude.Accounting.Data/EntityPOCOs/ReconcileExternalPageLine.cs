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
   
    public class ReconcileExternalPageLine
    {
	 string dbms;

           [ForeignKey("ReconcileExternalPage")]
        [Column("ReconcileExternalPageId" ,Order = 1)]
	    public string ReconcileExternalPageId { get; set; }
	      
        public virtual ReconcileExternalPage ReconcileExternalPage { get; set; }
        [Column("LineNumber" ,Order = 2)]
	    public int LineNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("DebitAmount")]
	    public decimal DebitAmount { get; set; }
        [Column("ReferenceDate")]
	    public DateTime ReferenceDate { get; set; }
        [Column("Reference")]
	    public string Reference { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("IsReconciled")]
	    public bool IsReconciled { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("ReconcileRemarks")]
	    public string ReconcileRemarks { get; set; }
        [Column("CreditAmount")]
	    public decimal CreditAmount { get; set; }
        [Column("InProgressExternalReconcile")]
	    public bool InProgressExternalReconcile { get; set; }
        [Column("InReconcileProgress")]
	    public bool InReconcileProgress { get; set; }
    }
}
	 