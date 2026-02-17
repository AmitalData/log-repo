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
   
    public class ReconcileExternalPage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("BankAccount")]
        [Column("BankAccountId")]
	    public string BankAccountId { get; set; }
	      
        public virtual BankAccount BankAccount { get; set; }
        [ForeignKey("GLAccount")]
        [Column("GLAccountId")]
	    public string GLAccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [Column("PageNo")]
	    public int PageNo { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("FromDate")]
	    public DateTime FromDate { get; set; }
        [Column("ToDate")]
	    public DateTime ToDate { get; set; }
        [Column("StartBalance")]
	    public decimal StartBalance { get; set; }
        [Column("CloseBalance")]
	    public decimal CloseBalance { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("ApprovedByUser")]
        [Column("ApprovedByUserId")]
	    public string ApprovedByUserId { get; set; }
	      
        public virtual User ApprovedByUser { get; set; }
        [ForeignKey("ReconcileExternalPageStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual ReconcileExternalPageStatus ReconcileExternalPageStatus { get; set; }
        [ForeignKey("BankPageEntryType")]
        [Column("EntryTypeCode")]
	    public string EntryTypeCode { get; set; }
	      
        public virtual BankPageEntryType BankPageEntryType { get; set; }
    }
}
	 