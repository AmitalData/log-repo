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
   
    public class Journal
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("JournalNumber")]
	    public string JournalNumber { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("AccountingDate")]
	    public DateTime AccountingDate { get; set; }
        [ForeignKey("JournalType")]
        [Column("TypeCode")]
	    public string TypeCode { get; set; }
	      
        public virtual JournalType JournalType { get; set; }
        [ForeignKey("JournalStatusType")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual JournalStatusType JournalStatusType { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("AccountingEntity")]
        [Column("AccountingEntityCode")]
	    public string AccountingEntityCode { get; set; }
	      
        public virtual AccountingEntity AccountingEntity { get; set; }
        [Column("AccountingEntityId")]
	    public string AccountingEntityId { get; set; }
        [Column("ExternalNo")]
	    public string ExternalNo { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("ApproveDate")]
	    public DateTime? ApproveDate { get; set; }
        [ForeignKey("ApprovedByUser")]
        [Column("ApprovedByUserId")]
	    public string ApprovedByUserId { get; set; }
	      
        public virtual User ApprovedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("AccountingEntityReference")]
	    public string AccountingEntityReference { get; set; }
        [ForeignKey("OriginalJournal")]
        [Column("OriginalJournalId")]
	    public string OriginalJournalId { get; set; }
	      
        public virtual Journal OriginalJournal { get; set; }
        [ForeignKey("VoidedByUser")]
        [Column("VoidedByUserId")]
	    public string VoidedByUserId { get; set; }
	      
        public virtual User VoidedByUser { get; set; }
        [Column("VoidDate")]
	    public DateTime? VoidDate { get; set; }
        [Column("IsVoided")]
	    public bool? IsVoided { get; set; }
        [Column("VoidedByJournalId")]
	    public string VoidedByJournalId { get; set; }
        [Column("ExternalSystem")]
	    public string ExternalSystem { get; set; }
        [Column("QueueId")]
	    public string QueueId { get; set; }
        [Column("IsLedgerCreated")]
	    public bool IsLedgerCreated { get; set; }
        [Column("DocumentDate")]
	    public DateTime? DocumentDate { get; set; }
        [Column("DueDate")]
	    public DateTime? DueDate { get; set; }
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
        [Column("SecurityLevel")]
	    public int? SecurityLevel { get; set; }
    }
}
	 