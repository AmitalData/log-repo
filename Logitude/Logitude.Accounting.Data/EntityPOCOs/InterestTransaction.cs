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
   
    public class InterestTransaction
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDateTime")]
	    public DateTime CreateDateTime { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime UpdateDateTime { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("GLAccount")]
        [Column("GLAccountId")]
	    public string GLAccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [ForeignKey("InterestEntityType")]
        [Column("InterestEntityTypeCode")]
	    public string InterestEntityTypeCode { get; set; }
	      
        public virtual InterestEntityType InterestEntityType { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [Column("OriginalEntityLineNumber")]
	    public int OriginalEntityLineNumber { get; set; }
        [Column("LocalAmount")]
	    public decimal LocalAmount { get; set; }
        [Column("ForeignAmount")]
	    public decimal? ForeignAmount { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("InterestValueDate")]
	    public DateTime InterestValueDate { get; set; }
        [Column("InterestReportId")]
	    public string InterestReportId { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
    }
}
	 