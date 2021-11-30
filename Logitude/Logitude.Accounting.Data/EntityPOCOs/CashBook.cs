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
   
    public class CashBook
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("EnglishName")]
	    public string EnglishName { get; set; }
        [Column("LocalName")]
	    public string LocalName { get; set; }
        [Column("Inactive")]
	    public bool? Inactive { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [ForeignKey("CashBookType")]
        [Column("CashBookTypeCode")]
	    public string CashBookTypeCode { get; set; }
	      
        public virtual CashBookType CashBookType { get; set; }
        [Column("TotalAmount")]
	    public decimal? TotalAmount { get; set; }
        [ForeignKey("Account")]
        [Column("AccountId")]
	    public string AccountId { get; set; }
	      
        public virtual GLAccount Account { get; set; }
        [ForeignKey("Branch")]
        [Column("BranchId")]
	    public string BranchId { get; set; }
	      
        public virtual Branch Branch { get; set; }
        [Column("InDepositingProgress")]
	    public bool InDepositingProgress { get; set; }
    }
}
	 